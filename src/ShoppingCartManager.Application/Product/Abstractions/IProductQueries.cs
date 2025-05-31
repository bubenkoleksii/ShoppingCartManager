namespace ShoppingCartManager.Application.Product.Abstractions;

using Product = Domain.Entities.Product;

public interface IProductQueries
{
    Task<Option<Product>> GetById(Guid userId, Guid productId, CancellationToken cancellationToken);

    Task<(IEnumerable<Product> Products, int TotalCount)> Get(
        Guid userId,
        int skip,
        int take,
        CancellationToken cancellationToken
    );

    Task<List<Product>> GetStats(
        Guid userId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    );
}
