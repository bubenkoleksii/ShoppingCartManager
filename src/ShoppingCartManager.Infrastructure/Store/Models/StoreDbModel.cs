namespace ShoppingCartManager.Infrastructure.Store.Models;

using Store = Domain.Entities.Store;

[Table(nameof(Store))]
public sealed class StoreDbModel : DbModelBase<Store>
{
    public const string TableName = nameof(Store);

    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }

    public StoreDbModel() { }

    public StoreDbModel(Store store)
        : base(store) { }

    protected override void MapFromDomainEntityCore(Store store)
    {
        UserId = store.UserId;
        Name = store.Name;
        Color = store.Color;
    }

    protected override Store MapToDomainEntityCore() =>
        new()
        {
            UserId = UserId,
            Name = Name,
            Color = Color,
        };
}
