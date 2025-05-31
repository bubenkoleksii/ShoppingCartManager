namespace ShoppingCartManager.Application.Category.Abstractions;

using Category = Domain.Entities.Category;

public interface ICategoryCommands
{
    Task<Either<Error, Category>> Add(Category category, CancellationToken cancellationToken = default);
    Task<Either<Error, Category>> Update(Category category, CancellationToken cancellationToken = default);
    Task<Option<Error>> Delete(Guid id, Guid userId, CancellationToken cancellationToken = default);
}
