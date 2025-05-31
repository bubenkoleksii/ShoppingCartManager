namespace ShoppingCartManager.Application.Category.Models;

public sealed class UpdateCategoryRequest
{
    [Required]
    public required Guid Id { get; init; }

    [Required, MaxLength(100)]
    public required string Name { get; init; }

    public int? IconId { get; init; }
}
