using ShoppingCartManager.Application.Product.Models;

namespace ShoppingCartManager.Application.Product.Abstractions;

public interface IProductService
{
    Task<Either<Error, ProductListResponse>> Get(int skip, int take, CancellationToken cancellationToken = default);
    Task<Either<Error, ProductResponse>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<Either<Error, ProductResponse>> Create(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<Either<Error, ProductResponse>> Update(UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default);
    Task<Option<Error>> MarkAsInCart(Guid id, CancellationToken cancellationToken = default);
    Task<Option<Error>> RemoveFromCart(Guid id, CancellationToken cancellationToken = default);
}
