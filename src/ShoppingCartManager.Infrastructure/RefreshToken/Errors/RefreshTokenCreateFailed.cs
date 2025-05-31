namespace ShoppingCartManager.Infrastructure.RefreshToken.Errors;

public sealed record RefreshTokenCreateFailed : ApiError
{
    public override string Title => nameof(RefreshTokenCreateFailed);
    public override string? ErrorMessage => null;
    public override string DefaultErrorMessage => "Failed to create refresh token";
}
