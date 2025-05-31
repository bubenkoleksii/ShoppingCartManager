namespace ShoppingCartManager.Application.Category.Errors;

public sealed record CategoryCreateFailedError : ApiError
{
    public override string Title => nameof(CategoryCreateFailedError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Failed to create category";

    public CategoryCreateFailedError(Guid userId)
    {
        ErrorMessage = $"Failed to create category for user with ID '{userId}'";
    }
}