namespace ShoppingCartManager.Infrastructure.User.Errors;

public sealed record UserUpdateFailed(Guid? UserId = null) : ApiError
{
    public override string Title => nameof(UserUpdateFailed);

    public override string? ErrorMessage =>
        UserId is not null ? $"Failed to update user with ID: {UserId}" : null;

    public override string DefaultErrorMessage => "Failed to update user";
}
