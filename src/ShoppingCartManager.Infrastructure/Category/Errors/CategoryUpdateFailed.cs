namespace ShoppingCartManager.Infrastructure.Category.Errors;

public sealed record CategoryUpdateFailed(Guid UserId, Guid CategoryId) : ApiError
{
    public override string Title => nameof(CategoryUpdateFailed);
    public override string ErrorMessage => $"Failed to update category {CategoryId} for user {UserId}";
    public override string DefaultErrorMessage => "Failed to update category";
}
