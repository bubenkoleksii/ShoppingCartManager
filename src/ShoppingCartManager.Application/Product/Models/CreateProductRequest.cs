namespace ShoppingCartManager.Application.Product.Models;

public sealed class CreateProductRequest
{
    [Required, MaxLength(255)]
    public required string Name { get; init; }

    public Guid? CategoryId { get; init; }


    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }

    public Guid? StoreId { get; init; }
}
