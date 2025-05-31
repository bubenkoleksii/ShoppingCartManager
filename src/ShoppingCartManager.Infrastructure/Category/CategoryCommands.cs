using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Infrastructure.Category.Errors;
using ShoppingCartManager.Infrastructure.Category.Models;

namespace ShoppingCartManager.Infrastructure.Category;

using Category = Domain.Entities.Category;

public sealed class CategoryCommands(IDbConnection connection) : ICategoryCommands
{
    public async Task<Either<Error, Category>> Add(Category category, CancellationToken cancellationToken = default)
    {
        var model = new CategoryDbModel(category);

        try
        {
            var inserted = await connection.Add(CategoryDbModel.TableName, model);
            if (!inserted)
                return new CategoryCreateFailed(category.UserId);
        }
        catch
        {
            return new CategoryCreateFailed(category.UserId);
        }

        var addedOption = await connection.GetSingleBy<CategoryDbModel>(
            CategoryDbModel.TableName,
            nameof(CategoryDbModel.Id),
            model.Id
        );

        return addedOption.Match<Either<Error, Category>>(
            Some: m => m.ToDomainEntity(),
            None: () => new CategoryCreateFailed(category.UserId)
        );
    }

    public async Task<Either<Error, Category>> Update(Category category, CancellationToken cancellationToken = default)
    {
        var existingOption = await connection.GetSingleBy<CategoryDbModel>(
            CategoryDbModel.TableName,
            nameof(CategoryDbModel.Id),
            category.Id
        );

        if (existingOption.IsNone || existingOption.First().UserId != category.UserId)
            return new CategoryUpdateFailed(category.UserId, category.Id);

        var model = new CategoryDbModel(category) { UpdatedAt = DateTime.UtcNow };

        try
        {
            var updated = await connection.Update(CategoryDbModel.TableName, model);
            if (!updated)
                return new CategoryUpdateFailed(category.UserId, category.Id);
        }
        catch
        {
            return new CategoryUpdateFailed(category.UserId, category.Id);
        }

        var updatedOption = await connection.GetSingleBy<CategoryDbModel>(
            CategoryDbModel.TableName,
            nameof(CategoryDbModel.Id),
            model.Id
        );

        return updatedOption.Match<Either<Error, Category>>(
            Some: m => m.ToDomainEntity(),
            None: () => new CategoryUpdateFailed(category.UserId, category.Id)
        );
    }

    public async Task<Option<Error>> Delete(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var existingOption = await connection.GetSingleBy<CategoryDbModel>(
            CategoryDbModel.TableName,
            nameof(CategoryDbModel.Id),
            id
        );

        if (existingOption.IsNone || existingOption.First().UserId != userId)
            return new CategoryDeleteFailed(userId, id);

        try
        {
            var deleted = await connection.DeleteById(CategoryDbModel.TableName, id);
            return deleted
                ? Option<Error>.None
                : new CategoryDeleteFailed(userId, id);
        }
        catch
        {
            return new CategoryDeleteFailed(userId, id);
        }
    }
}
