namespace ShoppingCartManager.Infrastructure.Store.Errors;

public sealed record StoreUpdateFailed(Guid UserId, Guid StoreId) : ApiError
{
    public override string Title => nameof(StoreUpdateFailed);
    public override string ErrorMessage => $"Failed to update store {StoreId} for user {UserId}";
    public override string DefaultErrorMessage => "Failed to update store";
}
