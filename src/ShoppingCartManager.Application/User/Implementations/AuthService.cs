using ShoppingCartManager.Application.Common.Abstractions;
using ShoppingCartManager.Application.RefreshToken.Abstractions;
using ShoppingCartManager.Application.Security;
using ShoppingCartManager.Application.Security.Jwt;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Errors;
using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.User.Implementations;

public class AuthService(
    IUserCommands userCommands,
    IUserQueries userQueries,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenCommands refreshTokenCommands,
    IRefreshTokenQueries refreshTokenQueries,
    IJwtTokenGenerator jwtTokenGenerator,
    IUserContext userContext,
    ILogger<AuthService> logger,
    IEnumerable<IOnUserAddedHandler> userAddedHandlers
) : IAuthService
{
    public async Task<Either<Error, AuthResult>> Register(
        RegisterUserRequest userRequest,
        CancellationToken cancellationToken = default
    )
    {
        var emailAlreadyExists = await userQueries.EmailExists(
            userRequest.Email,
            cancellationToken
        );
        if (emailAlreadyExists)
            return new EmailAlreadyExistsError(userRequest.Email);

        var (passwordHash, passwordSalt) = PasswordHasher.Hash(userRequest.Password);

        var newUser = new Domain.Entities.User
        {
            FullName = userRequest.FullName,
            Email = userRequest.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        var addUserResult = await userCommands.Add(newUser, cancellationToken);

        return await addUserResult.MatchAsync(
            async addedUser =>
            {
                var accessToken = jwtTokenGenerator.GenerateToken(addedUser);
                var refreshToken = refreshTokenGenerator.Generate(addedUser.Id);

                logger.LogInformation(
                    "Register: Generated refresh token for user {UserId}",
                    addedUser.Id
                );

                var saveError = await refreshTokenCommands.Add(refreshToken, cancellationToken);
                if (saveError.IsSome)
                    return saveError.First();

                foreach (var handler in userAddedHandlers)
                {
                    await handler.Handle(addedUser.Id, cancellationToken);
                }

                return new AuthResult(addedUser, accessToken, refreshToken.Token);
            },
            error => Task.FromResult<Either<Error, AuthResult>>(error)
        );
    }

    public async Task<Either<Error, AuthResult>> Login(
        LoginRequest loginRequest,
        CancellationToken cancellationToken = default
    )
    {
        var userOption = await userQueries.GetByEmail(loginRequest.Email, cancellationToken);

        return await userOption.Match(
            Some: async user =>
            {
                var isPasswordValid = PasswordHasher.Verify(
                    loginRequest.Password,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                if (!isPasswordValid)
                    return new InvalidCredentialsError();

                var accessToken = jwtTokenGenerator.GenerateToken(user);
                var refreshToken = refreshTokenGenerator.Generate(user.Id);

                logger.LogInformation("Login: Issued new refresh token for user {UserId}", user.Id);

                var saveError = await refreshTokenCommands.Add(refreshToken, cancellationToken);
                if (saveError.IsSome)
                    return saveError.First();

                return new AuthResult(user, accessToken, refreshToken.Token);
            },
            None: () => Task.FromResult<Either<Error, AuthResult>>(new InvalidCredentialsError())
        );
    }

    public async Task<Either<Error, AuthResult>> RefreshToken(
        string refreshToken,
        CancellationToken cancellationToken = default
    )
    {
        var existingTokenOption = await refreshTokenQueries.GetByToken(
            refreshToken,
            cancellationToken
        );

        return await existingTokenOption.MatchAsync(
            Some: async existingToken =>
            {
                if (!existingToken.IsActive)
                {
                    logger.LogWarning(
                        "Refresh token is expired or revoked: {RefreshToken}",
                        "[hidden]"
                    );
                    return new InvalidRefreshTokenError();
                }

                var userIdFromContext = userContext.UserId;
                if (
                    userIdFromContext is not { } currentUserId
                    || currentUserId != existingToken.UserId
                )
                {
                    logger.LogWarning(
                        "User {UserId} tried to refresh token not belonging to them",
                        userIdFromContext
                    );
                    return new InvalidRefreshTokenError();
                }

                var userOption = await userQueries.GetById(existingToken.UserId, cancellationToken);
                if (userOption.IsNone)
                {
                    logger.LogWarning(
                        "Refresh token used for unknown user: {UserId}",
                        existingToken.UserId
                    );
                    return new InvalidRefreshTokenError();
                }

                var user = userOption.First();
                var newRefreshToken = refreshTokenGenerator.Generate(user.Id);

                var revokeError = await refreshTokenCommands.Revoke(
                    refreshToken,
                    cancellationToken
                );
                if (revokeError.IsSome)
                    return revokeError.First();

                var saveError = await refreshTokenCommands.Add(newRefreshToken, cancellationToken);
                if (saveError.IsSome)
                    return saveError.First();

                var replaceError = await refreshTokenCommands.MarkReplacedBy(
                    refreshToken,
                    newRefreshToken.Token,
                    cancellationToken
                );
                if (replaceError.IsSome)
                    return replaceError.First();

                var accessToken = jwtTokenGenerator.GenerateToken(user);

                return new AuthResult(user, accessToken, newRefreshToken.Token);
            },
            None: () => Task.FromResult<Either<Error, AuthResult>>(new InvalidRefreshTokenError())
        );
    }
}
