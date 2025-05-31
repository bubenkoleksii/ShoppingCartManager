using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Infrastructure.Store.Models;

namespace ShoppingCartManager.Infrastructure.Store;

using Store = Domain.Entities.Store;

public sealed class StoreQueries(IDbConnection connection) : IStoreQueries
{
    public async Task<Option<Store>> GetById(
        Guid userId,
        Guid storeId,
        CancellationToken cancellationToken = default
    )
    {
        var dbStoreOption = await connection.GetSingleWhere<StoreDbModel>(
            StoreDbModel.TableName,
            new Dictionary<string, object>
            {
                [nameof(StoreDbModel.Id)] = storeId,
                [nameof(StoreDbModel.UserId)] = userId,
            }
        );

        return dbStoreOption.Match(
            Some: dbStore => Some(dbStore.ToDomainEntity()),
            None: () => Option<Store>.None
        );
    }

    public async Task<IEnumerable<Store>> Get(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var dbStores = await connection.GetAllWhere<StoreDbModel>(
            StoreDbModel.TableName,
            new Dictionary<string, object> { [nameof(StoreDbModel.UserId)] = userId }
        );

        return dbStores.Select(db => db.ToDomainEntity());
    }
}
