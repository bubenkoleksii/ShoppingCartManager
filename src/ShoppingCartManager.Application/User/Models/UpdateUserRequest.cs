namespace ShoppingCartManager.Application.User.Models;

public sealed class UpdateUserRequest
{
    [Required]
    public required Guid Id { get; init; }

    [MaxLength(150)]
    public string? FullName { get; init; }

    [Required, EmailAddress, MaxLength(255)]
    public required string Email { get; init; }
}
