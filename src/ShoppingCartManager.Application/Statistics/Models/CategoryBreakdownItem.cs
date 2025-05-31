namespace ShoppingCartManager.Application.Statistics.Models;

public sealed class CategoryBreakdownItem(
    Guid categoryId,
    string categoryName,
    decimal totalPrice,
    decimal inCartPrice
)
{
    public Guid CategoryId { get; init; } = categoryId;
    public string CategoryName { get; init; } = categoryName;
    public decimal TotalPrice { get; init; } = totalPrice;
    public decimal InCartPrice { get; init; } = inCartPrice;
}
