using ShoppingCartManager.API.Models.User;
using ShoppingCartManager.Application.RefreshToken.Models;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Models;

namespace ShoppingCartManager.API.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost(template: nameof(Register))]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.Register(request, cancellationToken);

        return result.Match(
            Right: authResult =>
                CreatedAtAction(
                    nameof(Register),
                    routeValues: new { id = authResult.User.Id },
                    new AuthResponse(authResult)
                ),
            ErrorActionResultHandler.Handle
        );
    }

    [HttpPost(template: nameof(Login))]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.Login(request, cancellationToken);

        return result.Match(
            Right: authResult => Ok(new AuthResponse(authResult)),
            ErrorActionResultHandler.Handle
        );
    }

    [HttpPost(nameof(Refresh))]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshToken(request.RefreshToken, cancellationToken);

        return result.Match(
            Right: authResult => Ok(new AuthResponse(authResult)),
            Left: ErrorActionResultHandler.Handle
        );
    }
}
