namespace ShoppingCartManager.Application.Common.Errors;

public sealed record UserNotFoundError : ApiError
{
    public override string Title => nameof(UserNotFoundError);

    public override string? ErrorMessage { get; }

    public override string DefaultErrorMessage => "User not found";

    public UserNotFoundError() { }

    public UserNotFoundError(Guid userId)
    {
        ErrorMessage = $"User with ID '{userId}' was not found";
    }

    public UserNotFoundError(string email)
    {
        ErrorMessage = $"User with email '{email}' was not found";
    }
}
