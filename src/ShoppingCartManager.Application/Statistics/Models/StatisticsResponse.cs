namespace ShoppingCartManager.Application.Statistics.Models;

public sealed class StatisticsResponse(
    DateTime from,
    DateTime to,
    int totalProducts,
    int productsInCart,
    IEnumerable<CategoryBreakdownItem> categoryBreakdown,
    IEnumerable<ProductBreakdownItem> productBreakdown)
{
    public DateTime From { get; init; } = from;
    public DateTime To { get; init; } = to;

    public int TotalProducts { get; init; } = totalProducts;
    public int ProductsInCart { get; init; } = productsInCart;

    public IEnumerable<CategoryBreakdownItem> CategoryBreakdown { get; init; } = categoryBreakdown;
    public IEnumerable<ProductBreakdownItem> ProductBreakdown { get; init; } = productBreakdown;
}
