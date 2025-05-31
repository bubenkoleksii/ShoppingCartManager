namespace ShoppingCartManager.Domain.Entities;

public class Category : EntityBase
{
    public required Guid UserId { get; set; }

    public required string Name { get; set; }

    public int? IconId { get; set; }
}
