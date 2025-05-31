using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Infrastructure.Category.Models;

namespace ShoppingCartManager.Infrastructure.Category;

using Category = Domain.Entities.Category;

public sealed class CategoryQueries(IDbConnection connection) : ICategoryQueries
{
    public async Task<Option<Category>> GetById(
        Guid userId,
        Guid categoryId,
        CancellationToken cancellationToken = default
    )
    {
        var dbCategoryOption = await connection.GetSingleWhere<CategoryDbModel>(
            CategoryDbModel.TableName,
            new Dictionary<string, object>
            {
                [nameof(CategoryDbModel.Id)] = categoryId,
                [nameof(CategoryDbModel.UserId)] = userId,
            }
        );

        return dbCategoryOption.Match(
            Some: dbCategory => Some(dbCategory.ToDomainEntity()),
            None: () => Option<Category>.None
        );
    }

    public async Task<IEnumerable<Category>> Get(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var dbCategories = await connection.GetAllWhere<CategoryDbModel>(
            CategoryDbModel.TableName,
            new Dictionary<string, object> { [nameof(CategoryDbModel.UserId)] = userId }
        );

        return dbCategories.Select(db => db.ToDomainEntity());
    }
}
