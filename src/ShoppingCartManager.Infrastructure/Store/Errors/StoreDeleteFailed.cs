namespace ShoppingCartManager.Infrastructure.Store.Errors;

public sealed record StoreDeleteFailed(Guid UserId, Guid StoreId) : ApiError
{
    public override string Title => nameof(StoreDeleteFailed);
    public override string ErrorMessage => $"Failed to delete store {StoreId} for user {UserId}";
    public override string DefaultErrorMessage => "Failed to delete store";
}
