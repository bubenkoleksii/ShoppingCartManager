using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Infrastructure.Store.Errors;
using ShoppingCartManager.Infrastructure.Store.Models;

namespace ShoppingCartManager.Infrastructure.Store;

using Store = Domain.Entities.Store;

public sealed class StoreCommands(IDbConnection connection) : IStoreCommands
{
    public async Task<Either<Error, Store>> Add(
        Store store,
        CancellationToken cancellationToken = default
    )
    {
        var model = new StoreDbModel(store);

        try
        {
            var inserted = await connection.Add(StoreDbModel.TableName, model);
            if (!inserted)
                return new StoreCreateFailed(store.UserId);
        }
        catch
        {
            return new StoreCreateFailed(store.UserId);
        }

        var addedOption = await connection.GetSingleBy<StoreDbModel>(
            StoreDbModel.TableName,
            nameof(StoreDbModel.Id),
            model.Id
        );

        return addedOption.Match<Either<Error, Store>>(
            Some: m => m.ToDomainEntity(),
            None: () => new StoreCreateFailed(store.UserId)
        );
    }

    public async Task<Either<Error, Store>> Update(
        Store store,
        CancellationToken cancellationToken = default
    )
    {
        var existingOption = await connection.GetSingleBy<StoreDbModel>(
            StoreDbModel.TableName,
            nameof(StoreDbModel.Id),
            store.Id
        );

        if (existingOption.IsNone || existingOption.First().UserId != store.UserId)
            return new StoreUpdateFailed(store.UserId, store.Id);

        var model = new StoreDbModel(store) { UpdatedAt = DateTime.UtcNow };

        try
        {
            var updated = await connection.Update(StoreDbModel.TableName, model);
            if (!updated)
                return new StoreUpdateFailed(store.UserId, store.Id);
        }
        catch
        {
            return new StoreUpdateFailed(store.UserId, store.Id);
        }

        var updatedOption = await connection.GetSingleBy<StoreDbModel>(
            StoreDbModel.TableName,
            nameof(StoreDbModel.Id),
            model.Id
        );

        return updatedOption.Match<Either<Error, Store>>(
            Some: m => m.ToDomainEntity(),
            None: () => new StoreUpdateFailed(store.UserId, store.Id)
        );
    }

    public async Task<Option<Error>> Delete(
        Guid storeId,
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var existingOption = await connection.GetSingleBy<StoreDbModel>(
            StoreDbModel.TableName,
            nameof(StoreDbModel.Id),
            storeId
        );

        if (existingOption.IsNone || existingOption.First().UserId != userId)
            return Option<Error>.None;

        try
        {
            var deleted = await connection.DeleteById(StoreDbModel.TableName, storeId);
            return deleted ? Option<Error>.None : new StoreDeleteFailed(userId, storeId);
        }
        catch
        {
            return new StoreDeleteFailed(userId, storeId);
        }
    }
}
