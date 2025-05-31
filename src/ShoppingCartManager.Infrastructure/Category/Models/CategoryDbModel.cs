namespace ShoppingCartManager.Infrastructure.Category.Models;

using Category = Domain.Entities.Category;

[Table(nameof(Category))]
public sealed class CategoryDbModel : DbModelBase<Category>
{
    public const string TableName = nameof(Category);

    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? IconId { get; set; }

    public CategoryDbModel() { }

    public CategoryDbModel(Category category)
        : base(category) { }

    protected override void MapFromDomainEntityCore(Category category)
    {
        UserId = category.UserId;
        Name = category.Name;
        IconId = category.IconId;
    }

    protected override Category MapToDomainEntityCore() =>
        new()
        {
            UserId = UserId,
            Name = Name,
            IconId = IconId
        };
}
