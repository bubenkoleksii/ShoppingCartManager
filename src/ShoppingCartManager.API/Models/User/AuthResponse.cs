using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.API.Models.User;

public sealed record AuthResponse
{
    public UserResponse User { get; init; }
    public string Token { get; init; }
    public string RefreshToken { get; init; }

    public AuthResponse(AuthResult authResult)
    {
        User = new UserResponse(authResult.User);
        Token = authResult.Token;
        RefreshToken = authResult.RefreshToken;
    }
}
