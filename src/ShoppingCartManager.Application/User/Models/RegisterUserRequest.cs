namespace ShoppingCartManager.Application.User.Models;

public sealed class RegisterUserRequest
{
    [Required, MaxLength(150)]
    public required string FullName { get; init; }

    [Required, EmailAddress, MaxLength(255)]
    public required string Email { get; init; }

    [Required, MinLength(6)]
    public required string Password { get; init; }
}
