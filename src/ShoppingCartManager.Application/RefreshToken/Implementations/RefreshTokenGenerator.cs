using System.Security.Cryptography;
using ShoppingCartManager.Application.RefreshToken.Abstractions;

namespace ShoppingCartManager.Application.RefreshToken.Implementations;

using RefreshToken = Domain.Entities.RefreshToken;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private const int RefreshTokenExpiryDays = 30;

    public RefreshToken Generate(Guid userId)
    {
        var token = Convert.ToBase64String(inArray: RandomNumberGenerator.GetBytes(count: 64));
        var expiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);

        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
        };
    }
}
