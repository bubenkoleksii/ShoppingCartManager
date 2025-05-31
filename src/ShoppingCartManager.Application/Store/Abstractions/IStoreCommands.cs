namespace ShoppingCartManager.Application.Store.Abstractions;

using Store = Domain.Entities.Store;

public interface IStoreCommands
{
    Task<Either<Error, Store>> Add(Store store, CancellationToken cancellationToken = default);
    Task<Either<Error, Store>> Update(Store store, CancellationToken cancellationToken = default);
    Task<Option<Error>> Delete(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
