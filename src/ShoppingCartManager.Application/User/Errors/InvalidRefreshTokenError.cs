namespace ShoppingCartManager.Application.User.Errors;

public sealed record InvalidRefreshTokenError : ValidationError
{
    public override string ErrorMessage => "Refresh token is invalid or expired.";
    public override string DefaultErrorMessage => "Invalid refresh token.";
}
