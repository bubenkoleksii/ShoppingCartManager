namespace ShoppingCartManager.Application.Category.Errors;

public sealed record CategoryNotFoundError : ApiError
{
    public override string Title => nameof(CategoryNotFoundError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Category not found";

    public CategoryNotFoundError(Guid categoryId)
    {
        ErrorMessage = $"Category with ID '{categoryId}' was not found";
    }
}
