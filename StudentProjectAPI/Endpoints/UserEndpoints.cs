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
                .WithTags("Utilisateurs")
                .WithOpenApi();

            group.MapGet("/", async ([FromServices] UserController controller) =>
            {
                var users = await controller.GetUsers();
                return Results.Ok(users);
            })
            .WithName("GetAllUsers")
            .WithSummary("Récupérer tous les utilisateurs")
            .WithDescription("Retourne la liste de tous les utilisateurs du système.")
            .Produces<List<UserDto>>(StatusCodes.Status200OK);

            group.MapGet("/{id}", async (string id, [FromServices] UserController controller) =>
            {
                var user = await controller.GetUser(id);
                return user is null ? Results.NotFound() : Results.Ok(user);
            })
            .WithName("GetUserById")
            .WithSummary("Récupérer un utilisateur par son ID")
            .WithDescription("Retourne les informations d'un utilisateur spécifique selon son identifiant.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id}", async (string id, [FromBody] UpdateUserDto updateDto, [FromServices] UserController controller) =>
            {
                var user = await controller.UpdateUser(id, updateDto);
                return user is null ? Results.NotFound() : Results.Ok(user);
            })
            .WithName("UpdateUser")
            .WithSummary("Mettre à jour un utilisateur")
            .WithDescription("Met à jour les informations d'un utilisateur existant.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id}", async (string id, [FromServices] UserController controller) =>
            {
                var deleted = await controller.DeleteUser(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteUser")
            .WithSummary("Supprimer un utilisateur")
            .WithDescription("Supprime un utilisateur existant selon son identifiant.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/stats", async ([FromServices] UserController controller) =>
            {
                var stats = await controller.GetUserStats();
                return Results.Ok(stats);
            })
            .WithName("GetUserStats")
            .WithSummary("Récupérer les statistiques des utilisateurs")
            .WithDescription("Retourne des statistiques globales sur les utilisateurs du système.")
            .Produces<UserStatsDto>(StatusCodes.Status200OK);
        }
    }
} 