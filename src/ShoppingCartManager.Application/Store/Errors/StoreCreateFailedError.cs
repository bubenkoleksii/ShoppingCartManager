namespace ShoppingCartManager.Application.Store.Errors;

public sealed record StoreCreateFailedError : ApiError
{
    public override string Title => nameof(StoreCreateFailedError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Failed to create store";

    public StoreCreateFailedError(Guid userId)
    {
        ErrorMessage = $"Failed to create store for user with ID '{userId}'";
    }
}
