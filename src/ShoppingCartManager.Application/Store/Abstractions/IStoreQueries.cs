namespace ShoppingCartManager.Application.Store.Abstractions;

using Store = Domain.Entities.Store;

public interface IStoreQueries
{
    Task<Option<Store>> GetById(Guid userId, Guid storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Store>> Get(Guid userId, CancellationToken cancellationToken = default);
}
