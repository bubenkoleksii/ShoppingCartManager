namespace ShoppingCartManager.Application.Category.Abstractions;

using Category = Domain.Entities.Category;

public interface ICategoryQueries
{
    Task<Option<Category>> GetById(
        Guid userId,
        Guid categoryId,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<Category>> Get(Guid userId, CancellationToken cancellationToken = default);
}
