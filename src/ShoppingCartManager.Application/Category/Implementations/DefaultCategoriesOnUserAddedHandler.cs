using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Common.Abstractions;

namespace ShoppingCartManager.Application.Category.Implementations;

using Category = Domain.Entities.Category;

public sealed class DefaultCategoriesOnUserAddedHandler(
    ICategoryCommands categoryCommands
) : IOnUserAddedHandler
{
    private static readonly List<(string Name, int? Icon)> DefaultCategories =
    [
        ("Groceries", null),
        ("Household", 111),
        ("Health", 270)
    ];

    public async Task Handle(Guid userId, CancellationToken cancellationToken = default)
    {
        foreach (var (name, icon) in DefaultCategories)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                IconId = icon,
            };

            await categoryCommands.Add(category, cancellationToken);
        }
    }
}
