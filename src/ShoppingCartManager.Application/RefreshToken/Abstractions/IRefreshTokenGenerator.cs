namespace ShoppingCartManager.Application.RefreshToken.Abstractions;

using RefreshToken= Domain.Entities.RefreshToken;

public interface IRefreshTokenGenerator
{
    RefreshToken Generate(Guid userId);
}
