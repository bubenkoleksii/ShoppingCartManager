namespace ShoppingCartManager.Infrastructure.Product.Models;

using Product = Domain.Entities.Product;

[Table(nameof(Product))]
public sealed class ProductDbModel : DbModelBase<Product>
{
    public const string TableName = nameof(Product);

    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public Guid? StoreId { get; set; }
    public bool IsInCart { get; set; }
    public DateTime? InCartAt { get; set; }
    public decimal Price { get; set; }

    public ProductDbModel() { }

    public ProductDbModel(Product product)
        : base(product) { }

    protected override void MapFromDomainEntityCore(Product product)
    {
        UserId = product.UserId;
        Name = product.Name;
        CategoryId = product.CategoryId;
        StoreId = product.StoreId;
        IsInCart = product.IsInCart;
        InCartAt = product.InCartAt;
        Price = product.Price;
    }

    protected override Product MapToDomainEntityCore() =>
        new()
        {
            UserId = UserId,
            Name = Name,
            CategoryId = CategoryId,
            StoreId = StoreId,
            IsInCart = IsInCart,
            InCartAt = InCartAt,
            Price = Price,
        };
}
