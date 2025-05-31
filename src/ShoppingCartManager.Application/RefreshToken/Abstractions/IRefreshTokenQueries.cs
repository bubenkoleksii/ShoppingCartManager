namespace ShoppingCartManager.Application.RefreshToken.Abstractions;

using RefreshToken = Domain.Entities.RefreshToken;

public interface IRefreshTokenQueries
{
    Task<Option<RefreshToken>> GetByToken(
        string token,
        CancellationToken cancellationToken = default
    );
}
