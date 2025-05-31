namespace ShoppingCartManager.Domain.Entities;

public class User : EntityBase
{
    public string? FullName { get; set; }

    public required string Email { get; set; }

    public byte[] PasswordHash { get; set; } = [];

    public byte[] PasswordSalt { get; set; } = [];
}
