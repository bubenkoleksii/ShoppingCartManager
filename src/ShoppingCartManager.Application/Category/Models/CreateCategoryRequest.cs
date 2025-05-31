namespace ShoppingCartManager.Application.Category.Models;

public sealed class CreateCategoryRequest
{
    [Required, MaxLength(100)]
    public required string Name { get; init; }

    public int? IconId { get; init; }
}
