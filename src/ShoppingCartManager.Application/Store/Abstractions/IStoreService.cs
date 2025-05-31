using ShoppingCartManager.Application.Store.Models;

namespace ShoppingCartManager.Application.Store.Abstractions;

using Store = Domain.Entities.Store;

public interface IStoreService
{
    Task<Either<Error, Store>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Store>> GetAll(CancellationToken cancellationToken = default);
    Task<Either<Error, Store>> Create(CreateStoreRequest request, CancellationToken cancellationToken = default);
    Task<Either<Error, Store>> Update(UpdateStoreRequest request, CancellationToken cancellationToken = default);
    Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default);
}
