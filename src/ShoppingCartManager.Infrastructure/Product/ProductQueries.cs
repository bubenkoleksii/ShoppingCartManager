using ShoppingCartManager.Application.Product.Abstractions;

namespace ShoppingCartManager.Infrastructure.Product;

using Product = Domain.Entities.Product;

public sealed class ProductQueries(IDbConnection connection) : IProductQueries
{
    public async Task<Option<Product>> GetById(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        var result = await connection.GetSingleWhere<Product>(nameof(Product), where: new Dictionary<string, object>
        {
            [nameof(Product.Id)] = productId,
            [nameof(Product.UserId)] = userId
        });

        return result;
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> Get(Guid userId, int skip, int take, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await connection.QueryPaginated<Product>(
            tableName: nameof(Product),
            where: new Dictionary<string, object> { [nameof(Product.UserId)] = userId },
            orderBy: nameof(Product.CreatedAt),
            skip: skip,
            take: take
        );

        return (products, totalCount);
    }

    public async Task<List<Product>> GetStats(
        Guid userId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           
                                   SELECT Id, Name, CategoryId, StoreId, Price, IsInCart, CreatedAt, UpdatedAt, UserId
                                   FROM [Product]
                                   WHERE UserId = @UserId AND CreatedAt BETWEEN @From AND @To
                               
                           """;

        var result = await connection.QueryAsync<Product>(
            sql,
            new { UserId = userId, From = from, To = to }
        );

        return result.ToList();
    }
}
