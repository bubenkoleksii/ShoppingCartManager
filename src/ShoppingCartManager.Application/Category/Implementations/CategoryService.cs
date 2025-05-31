using ShoppingCartManager.Application.Category.Abstractions;
using ShoppingCartManager.Application.Category.Errors;
using ShoppingCartManager.Application.Category.Models;
using ShoppingCartManager.Application.Common.Errors;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.Category.Implementations;

using Category = Domain.Entities.Category;

public sealed class CategoryService(
    ICategoryQueries categoryQueries,
    ICategoryCommands categoryCommands,
    IUserContext userContext,
    ILogger<CategoryService> logger
) : ICategoryService
{
    public async Task<Either<Error, Category>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[CategoryService] Attempting to get category by ID: {CategoryId}", id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var option = await categoryQueries.GetById(userId.Value, id, cancellationToken);

        return option.ToEither<Error>(new CategoryNotFoundError(id)).Do(_ =>
        {
            logger.LogInformation("[CategoryService] Category with ID {CategoryId} found", id);
        }).MapLeft(error =>
        {
            logger.LogWarning("[CategoryService] Category with ID {CategoryId} not found", id);
            return error;
        });
    }

    public async Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[CategoryService] Fetching all categories for current user");

        var userId = GetUserId();
        if (userId is null)
            return [];

        var result = await categoryQueries.Get(userId.Value, cancellationToken);
        return result;
    }

    public async Task<Either<Error, Category>> Create(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[CategoryService] Creating category with name '{Name}'", request.Name);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var category = new Category
        {
            UserId = userId.Value,
            Name = request.Name,
            IconId = request.IconId
        };

        var result = await categoryCommands.Add(category, cancellationToken);

        return result.Match<Either<Error, Category>>(
            Right: added =>
            {
                logger.LogInformation("[CategoryService] Category created with ID {CategoryId} for user {UserId}", added.Id, added.UserId);
                return added;
            },
            Left: error =>
            {
                logger.LogError("[CategoryService] Failed to create category for user {UserId}: {Error}", userId, error);
                return error;
            });
    }

    public async Task<Either<Error, Category>> Update(UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[CategoryService] Updating category with ID {CategoryId}", request.Id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var existing = await categoryQueries.GetById(userId.Value, request.Id, cancellationToken);
        if (existing.IsNone)
        {
            logger.LogWarning("[CategoryService] Category with ID {CategoryId} not found for update", request.Id);
            return new CategoryNotFoundError(request.Id);
        }

        var category = existing.First();
        category.Name = request.Name;
        category.IconId = request.IconId;

        var result = await categoryCommands.Update(category, cancellationToken);

        return result.Match<Either<Error, Category>>(
            Right: updated =>
            {
                logger.LogInformation("[CategoryService] Category with ID {CategoryId} successfully updated", updated.Id);
                return updated;
            },
            Left: error =>
            {
                logger.LogError("[CategoryService] Failed to update category with ID {CategoryId}: {Error}", category.Id, error);
                return error;
            });
    }

    public async Task<Option<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[CategoryService] Deleting category with ID {CategoryId}", id);

        var userId = GetUserId();
        if (userId is null)
            return new UserNotFoundError();

        var result = await categoryCommands.Delete(id, userId.Value, cancellationToken);

        if (result.IsNone)
        {
            logger.LogInformation("[CategoryService] Category with ID {CategoryId} deleted successfully", id);
        }
        else
        {
            logger.LogWarning("[CategoryService] Failed to delete category with ID {CategoryId}", id);
        }

        return result;
    }

    private Guid? GetUserId()
    {
        var userId = userContext.UserId;

        if (userId is null)
        {
            logger.LogWarning("[CategoryService] User ID is null in context");
        }

        return userId;
    }
}
