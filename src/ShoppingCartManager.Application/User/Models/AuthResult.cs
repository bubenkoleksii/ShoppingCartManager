namespace ShoppingCartManager.Application.User.Models;

using User = Domain.Entities.User;

public sealed record AuthResult(User User, string Token, string RefreshToken);
