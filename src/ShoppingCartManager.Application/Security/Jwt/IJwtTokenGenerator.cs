namespace ShoppingCartManager.Application.Security.Jwt;

using User = Domain.Entities.User;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
