using ShoppingCartManager.Application.Product.Abstractions;
using ShoppingCartManager.Infrastructure.Product.Errors;

namespace ShoppingCartManager.Infrastructure.Product;

using Product = Domain.Entities.Product;

public sealed class ProductCommands(IDbConnection connection) : IProductCommands
{
    public async Task<Either<Error, Product>> Add(
        Product product,
        CancellationToken cancellationToken
    )
    {
        var success = await connection.Add(nameof(Product), product);

        return success ? Right(product) : new ProductCreateFailedError(product.Id);
    }

    public async Task<Either<Error, Product>> Update(
        Product product,
        CancellationToken cancellationToken
    )
    {
        var success = await connection.Update(nameof(Product), product);

        return success ? Right(product) : new ProductUpdateFailedError(product.UserId, product.Id);
    }

    public async Task<Option<Error>> Delete(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var existing = await connection.GetSingleWhere<Product>(
            nameof(Product),
            where: new Dictionary<string, object>
            {
                [nameof(Product.Id)] = productId,
                [nameof(Product.UserId)] = userId,
            }
        );

        if (existing.IsNone)
            return new ProductNotFoundError(productId);

        await connection.DeleteById(nameof(Product), productId);
        return Option<Error>.None;
    }

    public async Task<Option<Error>> MarkAsInCart(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var existing = await connection.GetSingleWhere<Product>(
            nameof(Product),
            new Dictionary<string, object>
            {
                [nameof(Product.Id)] = productId,
                [nameof(Product.UserId)] = userId,
            }
        );

        if (existing.IsNone)
            return new ProductNotFoundError(productId);

        var product = existing.First();
        product.IsInCart = true;
        product.InCartAt = DateTime.UtcNow;

        var updated = await connection.Update(nameof(Product), product);
        return updated
            ? Option<Error>.None
            : new ProductUpdateFailedError(product.UserId, product.Id);
    }

    public async Task<Option<Error>> RemoveFromCart(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var existing = await connection.GetSingleWhere<Product>(
            nameof(Product),
            new Dictionary<string, object>
            {
                [nameof(Product.Id)] = productId,
                [nameof(Product.UserId)] = userId,
            }
        );

        if (existing.IsNone)
            return new ProductNotFoundError(productId);

        var product = existing.First();
        product.IsInCart = false;
        product.InCartAt = null;

        var updated = await connection.Update(nameof(Product), product);
        return updated
            ? Option<Error>.None
            : new ProductUpdateFailedError(product.UserId, product.Id);
    }
}
