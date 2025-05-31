namespace ShoppingCartManager.Application.User.Errors;

public sealed record EmailAlreadyExistsError(string Email) : ValidationError
{
    public override string ErrorMessage => $"User with email '{Email}' already exists";
    public override string DefaultErrorMessage => "Email already exists";

    public override Dictionary<string, object> Details { get; init; } =
        new() { { "email", Email } };
}
