namespace ShoppingCartManager.Infrastructure.Product.Errors;

public sealed record ProductNotFoundError : ApiError
{
    public override string Title => nameof(ProductNotFoundError);

    public override string? ErrorMessage { get; }

    public override string DefaultErrorMessage => "Product not found";

    public ProductNotFoundError(Guid productId)
    {
        ErrorMessage = $"Product with ID '{productId}' was not found";
    }
}
