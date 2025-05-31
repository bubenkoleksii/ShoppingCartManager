namespace ShoppingCartManager.Infrastructure.User.Errors;

public sealed record UserDeleteFailed(Guid UserId) : ApiError
{
    public override string Title => nameof(UserDeleteFailed);
    public override string ErrorMessage => $"Failed to delete user with ID = {UserId}";
    public override string DefaultErrorMessage => "User deletion failed";
}
