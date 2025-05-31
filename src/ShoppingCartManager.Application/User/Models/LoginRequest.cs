namespace ShoppingCartManager.Application.User.Models;

public sealed class LoginRequest
{
    [Required, EmailAddress, MaxLength(255)]
    public required string Email { get; init; }

    [Required, MinLength(6)]
    public required string Password { get; init; }
}
