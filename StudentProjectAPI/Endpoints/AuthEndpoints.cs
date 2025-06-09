using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Controllers;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StudentProjectAPI.Endpoints;

/// Classe statique contenant les endpoints d'authentification
public static class AuthEndpoints
{
    /// Configure les endpoints d'authentification
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        // Création du groupe d'endpoints pour l'authentification
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        // POST /api/auth/register
        group.MapPost("/register", async (RegisterUserDto dto, [FromServices] AuthController controller) =>
        {
            var response = await controller.Register(dto);
            return Results.Ok(response);
        })
        .WithName("Register")
        .WithSummary("Inscription d'un nouvel utilisateur")
        .WithDescription("Permet à un nouvel utilisateur de s'inscrire dans le système")
        .Produces<AuthResponseDto>(200)
        .Produces(400);

        // POST /api/auth/login
        group.MapPost("/login", async (LoginUserDto dto, [FromServices] AuthController controller) =>
        {
            var response = await controller.Login(dto);
            return Results.Ok(response);
        })
        .WithName("Login")
        .WithSummary("Connexion d'un utilisateur")
        .WithDescription("Permet à un utilisateur de se connecter et d'obtenir un token JWT")
        .Produces<AuthResponseDto>(200)
        .Produces(400);

        // POST /api/auth/change-password
        group.MapPost("/change-password", async (ChangePasswordDto dto, [FromServices] AuthController controller, HttpContext context) =>
        {
            var userId = int.Parse(context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var response = await controller.ChangePassword(userId, dto);
            return Results.Ok(response);
        })
        .WithName("ChangePassword")
        .WithSummary("Changement de mot de passe")
        .WithDescription("Permet à un utilisateur connecté de changer son mot de passe")
        .RequireAuthorization()
        .Produces<AuthResponseDto>(200)
        .Produces(400)
        .Produces(401);
    }
}