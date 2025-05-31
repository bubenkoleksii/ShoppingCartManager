namespace ShoppingCartManager.Infrastructure.Category.Errors;

public sealed record CategoryNotFoundError(Guid Id) : ApiError
{
    public override string Title => nameof(CategoryNotFoundError);
    public override string ErrorMessage => $"Category with ID {Id} not found";
    public override string DefaultErrorMessage => "Category not found";
}
