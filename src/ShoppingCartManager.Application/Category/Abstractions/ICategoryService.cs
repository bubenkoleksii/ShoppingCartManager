using ShoppingCartManager.Application.Category.Models;

namespace ShoppingCartManager.Application.Category.Abstractions;

using Category = Domain.Entities.Category;

public interface ICategoryService
{
    Task<Either<Error, Category>> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken = default);
    Task<Either<Error, Category>> Create(
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default
    );
    Task<Either<Error, Category>> Update(
        UpdateCategoryRequest request,
        CancellationToken cancellationToken = default
    );
    Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default);
}
