using System.Security.Claims;
using ShoppingCartManager.API.Models.User;
using ShoppingCartManager.Application.User.Abstractions;
using ShoppingCartManager.Application.User.Models;
using ShoppingCartManager.Domain.Entities;

namespace ShoppingCartManager.API.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public sealed class UserController(IUserService userService) : ControllerBase
{
    [HttpGet(template: "{id:guid}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await userService.GetById(id, cancellationToken);

        return result.Match(
            Right: user => Ok(new UserResponse(user)),
            ErrorActionResultHandler.Handle
        );
    }

    [HttpGet(template: "email/{email}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var result = await userService.GetByEmail(email, cancellationToken);

        return result.Match(
            Right: user => Ok(new UserResponse(user)),
            ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpGet(template: nameof(Me))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await userService.GetById(userId, cancellationToken);

        return result.Match(
            Right: user => Ok(new UserResponse(user)),
            ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await userService.Update(request, cancellationToken);

        return result.Match(
            Right: user => Ok(new UserResponse(user)),
            Left: ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await userService.Delete(id, cancellationToken);

        return result.Match(None: NoContent, Some: ErrorActionResultHandler.Handle);
    }
}
