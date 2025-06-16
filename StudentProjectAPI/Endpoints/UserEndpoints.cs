using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StudentProjectAPI.Services;
using System.Security.Claims;

namespace StudentProjectAPI.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Users").RequireAuthorization();

        group.MapDelete("/me", async (HttpContext context, IUserService userService) =>
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Results.Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            var result = await userService.DeleteUserAsync(userId);

            return result ? Results.NoContent() : Results.NotFound();
        });
    }
}

