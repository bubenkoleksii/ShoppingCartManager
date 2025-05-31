using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ShoppingCartManager.Application.Security.Jwt;

using User = Domain.Entities.User;

public sealed class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
{
    private readonly JwtSettings _settings = options.Value;

    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new(type: JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(type: JwtRegisteredClaimNames.Name, user.Email),
            new(type: JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
