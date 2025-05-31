using ShoppingCartManager.Application.Common.Errors;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.User.Implementations;

using User = Domain.Entities.User;

public sealed class UserService(
    IUserQueries userQueries,
    IUserCommands userCommands,
    IUserContext userContext,
    ILogger<UserService> logger
) : IUserService
{
    public async Task<Either<Error, User>> GetById(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Fetching user by ID: {UserId}", id);

        var userOption = await userQueries.GetById(id, cancellationToken);

        return userOption
            .ToEither<Error>(new UserNotFoundError(id))
            .Do(_ => logger.LogInformation("User with ID {UserId} found", id))
            .MapLeft(error =>
            {
                logger.LogWarning("User with ID {UserId} not found", id);
                return error;
            });
    }

    public async Task<Either<Error, User>> GetByEmail(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Fetching user by email: {Email}", email);

        var userOption = await userQueries.GetByEmail(email, cancellationToken);

        return userOption
            .ToEither<Error>(new UserNotFoundError(email))
            .Do(_ => logger.LogInformation("User with email {Email} found", email))
            .MapLeft(error =>
            {
                logger.LogWarning("User with email {Email} not found", email);
                return error;
            });
    }

    public async Task<Either<Error, User>> Update(
        UpdateUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Updating user with ID {UserId}", request.Id);

        if (userContext.UserId is not { } currentUserId || currentUserId != request.Id)
        {
            logger.LogWarning(
                "Unauthorized update attempt by user {CurrentUserId} on user {TargetUserId}",
                userContext.UserId,
                request.Id
            );
            return new UserNotFoundError(request.Id);
        }

        var userResult = await GetById(request.Id, cancellationToken);
        if (userResult.IsLeft)
        {
            logger.LogWarning("User with ID {UserId} not found for update", request.Id);
            return userResult;
        }

        var user = userResult.RightAsEnumerable().First();
        user.FullName = request.FullName;
        user.Email = request.Email;

        var updateResult = await userCommands.Update(user, cancellationToken);

        return updateResult.Match<Either<Error, User>>(
            Right: updatedUser =>
            {
                logger.LogInformation("User with ID {UserId} successfully updated", updatedUser.Id);
                return updatedUser;
            },
            Left: error =>
            {
                logger.LogError(
                    "Failed to update user with ID {UserId}: {Error}",
                    request.Id,
                    error
                );
                return error;
            }
        );
    }

    public async Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting user with ID {UserId}", id);

        if (userContext.UserId is not { } currentUserId || currentUserId != id)
        {
            logger.LogWarning(
                "Unauthorized delete attempt by user {CurrentUserId} on user {TargetUserId}",
                userContext.UserId,
                id
            );
            return new UserNotFoundError(id);
        }

        var deleteResult = await userCommands.Delete(id, cancellationToken);

        if (deleteResult.IsNone)
        {
            logger.LogInformation("User with ID {UserId} successfully deleted", id);
        }
        else
        {
            logger.LogWarning("Failed to delete user with ID {UserId}", id);
        }

        return deleteResult;
    }
}
