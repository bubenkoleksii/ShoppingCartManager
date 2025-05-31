using ShoppingCartManager.Application.Common.Errors;
using ShoppingCartManager.Application.Store.Abstractions;
using ShoppingCartManager.Application.Store.Errors;
using ShoppingCartManager.Application.Store.Models;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Store.Implementations;

using Store = Domain.Entities.Store;

public sealed class StoreService(
    IStoreQueries storeQueries,
    IStoreCommands storeCommands,
    IUserContext userContext,
    ILogger<StoreService> logger
) : IStoreService
{
    public async Task<Either<Error, Store>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[StoreService] Attempting to get store by ID: {StoreId}", id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var option = await storeQueries.GetById(userId.Value, id, cancellationToken);

        return option.ToEither<Error>(new StoreNotFoundError(id)).Do(_ =>
        {
            logger.LogInformation("[StoreService] Store with ID {StoreId} found", id);
        }).MapLeft(error =>
        {
            logger.LogWarning("[StoreService] Store with ID {StoreId} not found", id);
            return error;
        });
    }

    public async Task<IEnumerable<Store>> GetAll(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[StoreService] Fetching all stores for current user");

        var userId = GetUserId();
        if (userId is null)
            return [];

        var result = await storeQueries.Get(userId.Value, cancellationToken);
        return result;
    }

    public async Task<Either<Error, Store>> Create(CreateStoreRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[StoreService] Creating store with name '{Name}'", request.Name);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var store = new Store
        {
            UserId = userId.Value,
            Name = request.Name,
            Color = request.Color
        };

        var result = await storeCommands.Add(store, cancellationToken);

        return result.Match<Either<Error, Store>>(
            Right: added =>
            {
                logger.LogInformation("[StoreService] Store created with ID {StoreId} for user {UserId}", added.Id, added.UserId);
                return added;
            },
            Left: error =>
            {
                logger.LogError("[StoreService] Failed to create store for user {UserId}: {Error}", userId, error);
                return error;
            });
    }

    public async Task<Either<Error, Store>> Update(UpdateStoreRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[StoreService] Updating store with ID {StoreId}", request.Id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var existing = await storeQueries.GetById(userId.Value, request.Id, cancellationToken);
        if (existing.IsNone)
        {
            logger.LogWarning("[StoreService] Store with ID {StoreId} not found for update", request.Id);
            return new StoreNotFoundError(request.Id);
        }

        var store = existing.First();
        store.Name = request.Name;
        store.Color = request.Color;

        var result = await storeCommands.Update(store, cancellationToken);

        return result.Match<Either<Error, Store>>(
            Right: updated =>
            {
                logger.LogInformation("[StoreService] Store with ID {StoreId} successfully updated", updated.Id);
                return updated;
            },
            Left: error =>
            {
                logger.LogError("[StoreService] Failed to update store with ID {StoreId}: {Error}", store.Id, error);
                return error;
            });
    }

    public async Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[StoreService] Deleting store with ID {StoreId}", id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var result = await storeCommands.Delete(id, userId.Value, cancellationToken);

        if (result.IsNone)
        {
            logger.LogInformation("[StoreService] Store with ID {StoreId} deleted successfully", id);
        }
        else
        {
            logger.LogWarning("[StoreService] Failed to delete store with ID {StoreId}", id);
        }

        return result;
    }

    private Guid? GetUserId()
    {
        var userId = userContext.UserId;

        if (userId is null)
            logger.LogWarning("[StoreService] User ID is null in context");

        return userId;
    }
}
