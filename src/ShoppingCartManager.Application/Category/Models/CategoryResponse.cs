namespace ShoppingCartManager.Application.Category.Models;

using Category = Domain.Entities.Category;

public record CategoryResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public int? IconId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public CategoryResponse(Category category)
    {
        Id = category.Id;
        UserId = category.UserId;
        Name = category.Name;
        IconId = category.IconId;
        CreatedAt = category.CreatedAt;
        UpdatedAt = category.UpdatedAt;
    }
}
