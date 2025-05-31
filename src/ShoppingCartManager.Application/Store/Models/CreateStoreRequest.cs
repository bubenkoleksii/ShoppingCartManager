namespace ShoppingCartManager.Application.Store.Models;

public sealed class CreateStoreRequest
{
    [Required, MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(50)]
    public string? Color { get; init; }
}