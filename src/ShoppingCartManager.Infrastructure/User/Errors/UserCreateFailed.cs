namespace ShoppingCartManager.Infrastructure.User.Errors;

public sealed record UserCreateFailed(string? Email = null) : ApiError
{
    public override string Title => nameof(UserCreateFailed);

    public override string? ErrorMessage =>
        !string.IsNullOrWhiteSpace(Email) ? $"Failed to create user with email: {Email}" : null;

    public override string DefaultErrorMessage => "Failed to create user";
}
