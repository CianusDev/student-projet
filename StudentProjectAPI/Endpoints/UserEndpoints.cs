using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Controllers;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/user")
                .WithTags("Users");

            group.MapGet("/", async ([FromServices] UserController controller) =>
            {
                var users = await controller.GetUsers();
                return Results.Ok(users);
            });

            group.MapGet("/{id}", async (int id, [FromServices] UserController controller) =>
            {
                var user = await controller.GetUser(id);
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateUserDto updateDto, [FromServices] UserController controller) =>
            {
                var user = await controller.UpdateUser(id, updateDto);
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            group.MapDelete("/{id}", async (int id, [FromServices] UserController controller) =>
            {
                var deleted = await controller.DeleteUser(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            group.MapGet("/stats", async ([FromServices] UserController controller) =>
            {
                var stats = await controller.GetUserStats();
                return Results.Ok(stats);
            });
        }
    }
} 