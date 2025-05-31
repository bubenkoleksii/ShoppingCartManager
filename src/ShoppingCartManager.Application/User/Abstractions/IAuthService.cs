using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.Application.User.Abstractions;

public interface IAuthService
{
    Task<Either<Error, AuthResult>> Register(
        RegisterUserRequest userRequest,
        CancellationToken cancellationToken = default
    );

    Task<Either<Error, AuthResult>> Login(
        LoginRequest request,
        CancellationToken cancellationToken = default
    );

    Task<Either<Error, AuthResult>> RefreshToken(
        string refreshToken,
        CancellationToken cancellationToken = default
    );
}
