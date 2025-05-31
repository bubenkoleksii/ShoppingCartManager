namespace ShoppingCartManager.Domain.Entities;

public class Store : EntityBase
{
    public required Guid UserId { get; set; }

    public required string Name { get; set; }

    public string? Color { get; set; }
}
