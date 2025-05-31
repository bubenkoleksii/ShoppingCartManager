namespace ShoppingCartManager.Application.User.Errors;

public sealed record InvalidCredentialsError : ValidationError
{
    public override string Title => nameof(InvalidCredentialsError);
    public override string ErrorMessage => "Invalid email or password";
    public override string DefaultErrorMessage => "Authentication failed";
}
