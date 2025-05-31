namespace ShoppingCartManager.Application.Category.Errors;

public sealed record CategoryUpdateFailedError : ApiError
{
    public override string Title => nameof(CategoryUpdateFailedError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Failed to update category";

    public CategoryUpdateFailedError(Guid userId, Guid categoryId)
    {
        ErrorMessage =
            $"Failed to update category with ID '{categoryId}' for user with ID '{userId}'";
    }
}
