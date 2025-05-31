namespace ShoppingCartManager.Application.Statistics.Models;

public sealed class ProductBreakdownItem(
    Guid productId,
    string productName,
    decimal price,
    bool isInCart,
    Guid categoryId,
    string categoryName,
    Guid? storeId,
    string? storeName
)
{
    public Guid ProductId { get; init; } = productId;
    public string ProductName { get; init; } = productName;
    public decimal Price { get; init; } = price;
    public bool IsInCart { get; init; } = isInCart;
    public Guid CategoryId { get; init; } = categoryId;
    public string CategoryName { get; init; } = categoryName;
    public Guid? StoreId { get; init; } = storeId;
    public string? StoreName { get; init; } = storeName;
}
