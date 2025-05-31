namespace ShoppingCartManager.Application.RefreshToken.Abstractions;

using RefreshToken = Domain.Entities.RefreshToken;

public interface IRefreshTokenCommands
{
    Task<Option<Error>> Add(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default
    );
    Task<Option<Error>> Revoke(string token, CancellationToken cancellationToken = default);
    Task<Option<Error>> MarkReplacedBy(
        string oldToken,
        string newToken,
        CancellationToken cancellationToken = default
    );
}
