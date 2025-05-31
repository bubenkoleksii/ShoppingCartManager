namespace ShoppingCartManager.Application.Product.Abstractions;

using Product = Domain.Entities.Product;

public interface IProductCommands
{
    Task<Either<Error, Product>> Add(Product product, CancellationToken cancellationToken);

    Task<Either<Error, Product>> Update(Product product, CancellationToken cancellationToken);

    Task<Option<Error>> Delete(Guid productId, Guid userId, CancellationToken cancellationToken);

    Task<Option<Error>> MarkAsInCart(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken
    );

    Task<Option<Error>> RemoveFromCart(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken
    );
}
