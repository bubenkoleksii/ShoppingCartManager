namespace ShoppingCartManager.Application.Store.Errors;

public sealed record StoreUpdateFailedError : ApiError
{
    public override string Title => nameof(StoreUpdateFailedError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Failed to update store";

    public StoreUpdateFailedError(Guid userId, Guid storeId)
    {
        ErrorMessage = $"Failed to update store with ID '{storeId}' for user with ID '{userId}'";
    }
}
