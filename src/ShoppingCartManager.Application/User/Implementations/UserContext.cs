using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShoppingCartManager.Application.User.Abstractions;

namespace ShoppingCartManager.Application.User.Implementations;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User.FindFirstValue(
                claimType: ClaimTypes.NameIdentifier
            );

            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }
    }
}
