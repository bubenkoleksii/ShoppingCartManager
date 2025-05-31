namespace ShoppingCartManager.Domain.Entities;

public sealed class Product : EntityBase
{
    public required Guid UserId { get; set; }

    public required string Name { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? StoreId { get; set; }

    public bool IsInCart { get; set; }

    public DateTime? InCartAt { get; set; }

    public decimal Price { get; set; }
}
