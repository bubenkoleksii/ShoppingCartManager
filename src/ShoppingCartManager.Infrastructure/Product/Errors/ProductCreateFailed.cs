namespace ShoppingCartManager.Infrastructure.Product.Errors;

public sealed record ProductCreateFailedError : ApiError
{
    public override string Title => nameof(ProductCreateFailedError);

    public override string? ErrorMessage { get; }

    public override string DefaultErrorMessage => "Failed to create product";

    public ProductCreateFailedError(Guid productId)
    {
        ErrorMessage = $"Failed to create product with ID '{productId}'";
    }
}
