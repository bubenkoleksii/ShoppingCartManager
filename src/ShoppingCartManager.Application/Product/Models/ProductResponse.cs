namespace ShoppingCartManager.Application.Product.Models;

using Product = Domain.Entities.Product;

public sealed class ProductResponse(Product product)
{
    public Guid Id { get; init; } = product.Id;
    public string Name { get; init; } = product.Name;
    public Guid? CategoryId { get; init; } = product.CategoryId;
    public Guid UserId { get; init; } = product.UserId;
    public Guid? StoreId { get; init; } = product.StoreId;
    public bool IsInCart { get; init; } = product.IsInCart;
    public DateTime? InCartAt { get; init; } = product.InCartAt;
    public decimal Price { get; init; } = product.Price;
    public DateTime CreatedAt { get; init; } = product.CreatedAt;
    public DateTime? UpdatedAt { get; init; } = product.UpdatedAt;
}
