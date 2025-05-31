namespace ShoppingCartManager.Infrastructure.Category.Errors;

public sealed record CategoryDeleteFailed(Guid UserId, Guid CategoryId) : ApiError
{
    public override string Title => nameof(CategoryDeleteFailed);
    public override string ErrorMessage => $"Failed to delete category {CategoryId} for user {UserId}";
    public override string DefaultErrorMessage => "Failed to delete category";
}
