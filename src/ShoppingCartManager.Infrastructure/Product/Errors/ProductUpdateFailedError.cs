namespace ShoppingCartManager.Infrastructure.Product.Errors;

public sealed record ProductUpdateFailedError : ApiError
{
    public override string Title => nameof(ProductUpdateFailedError);

    public override string? ErrorMessage { get; }

    public override string DefaultErrorMessage => "Failed to update product";

    public ProductUpdateFailedError(Guid userId, Guid productId)
    {
        ErrorMessage = $"Failed to update product with ID '{productId}' for user '{userId}'";
    }
}
