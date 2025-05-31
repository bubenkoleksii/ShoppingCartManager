namespace ShoppingCartManager.Infrastructure.RefreshToken.Errors;

public sealed record RefreshTokenReplaceFailed : ApiError
{
    public override string Title => nameof(RefreshTokenReplaceFailed);
    public override string? ErrorMessage => null;
    public override string DefaultErrorMessage => "Failed to replace refresh token";
}
