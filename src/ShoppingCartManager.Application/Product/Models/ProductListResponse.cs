namespace ShoppingCartManager.Application.Product.Models;

public sealed class ProductListResponse(IEnumerable<ProductResponse> products, int total, int take)
{
    public IEnumerable<ProductResponse> Products { get; init; } = products;
    public int Total { get; init; } = total;
    public int Take { get; init; } = take;
}
