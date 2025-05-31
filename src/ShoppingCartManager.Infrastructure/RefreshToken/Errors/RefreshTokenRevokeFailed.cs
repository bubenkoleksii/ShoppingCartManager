namespace ShoppingCartManager.Infrastructure.RefreshToken.Errors;

public sealed record RefreshTokenRevokeFailed : ApiError
{
    public override string Title => nameof(RefreshTokenRevokeFailed);
    public override string? ErrorMessage => null;
    public override string DefaultErrorMessage => "Failed to revoke refresh token";
}
