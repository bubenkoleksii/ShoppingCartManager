namespace ShoppingCartManager.Application.Store.Errors;

public sealed record StoreNotFoundError : ApiError
{
    public override string Title => nameof(StoreNotFoundError);
    public override string? ErrorMessage { get; }
    public override string DefaultErrorMessage => "Store not found";

    public StoreNotFoundError(Guid storeId)
    {
        ErrorMessage = $"Store with ID '{storeId}' was not found";
    }
}
