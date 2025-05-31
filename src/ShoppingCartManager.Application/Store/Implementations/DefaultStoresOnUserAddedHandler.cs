using ShoppingCartManager.Application.Common.Abstractions;
using ShoppingCartManager.Application.Store.Abstractions;

namespace ShoppingCartManager.Application.Store.Implementations;

using Store = Domain.Entities.Store;

public sealed class DefaultStoresOnUserAddedHandler(IStoreCommands storeCommands)
    : IOnUserAddedHandler
{
    private static readonly List<(string Name, string Color)> DefaultStores =
    [
        ("Local Supermarket", "#FF5722"),
        ("Neighborhood Grocery", "#4CAF50"),
        ("Artisan Bakery", "#FFC107"),
    ];

    public async Task Handle(Guid userId, CancellationToken cancellationToken = default)
    {
        foreach (var (name, color) in DefaultStores)
        {
            var store = new Store
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                Color = color,
            };

            await storeCommands.Add(store, cancellationToken);
        }
    }
}
