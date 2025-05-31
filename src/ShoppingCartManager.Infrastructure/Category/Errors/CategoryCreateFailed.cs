namespace ShoppingCartManager.Infrastructure.Category.Errors;

public sealed record CategoryCreateFailed(Guid UserId) : ApiError
{
    public override string Title => nameof(CategoryCreateFailed);
    public override string ErrorMessage => $"Failed to create category for user {UserId}";
    public override string DefaultErrorMessage => "Failed to create category";
}
