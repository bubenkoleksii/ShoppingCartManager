namespace ShoppingCartManager.Infrastructure.Store.Errors;

public sealed record StoreCreateFailed(Guid UserId) : ApiError
{
    public override string Title => nameof(StoreCreateFailed);
    public override string ErrorMessage => $"Failed to create store for user {UserId}";
    public override string DefaultErrorMessage => "Failed to create store";
}
