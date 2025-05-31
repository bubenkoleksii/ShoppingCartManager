namespace ShoppingCartManager.Application.Store.Models;

using Store = Domain.Entities.Store;

public sealed record StoreResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Color { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public StoreResponse(Store store)
    {
        Id = store.Id;
        Name = store.Name;
        Color = store.Color;
        UserId = store.UserId;
        CreatedAt = store.CreatedAt;
        UpdatedAt = store.UpdatedAt;
    }
}
