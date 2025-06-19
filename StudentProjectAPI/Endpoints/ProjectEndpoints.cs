using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Controllers;
using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Endpoints
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/projects")
                .WithTags("Projects")
                .WithOpenApi();

            group.MapPost("/", async (CreateProjectDto dto, [FromServices] ProjectController controller, HttpContext context) =>
            {
                try
                {
                    var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Results.Unauthorized();
                    }
                    var project = await controller.CreateProject(dto, userId);
                    return Results.Created($"/api/projects/{project.Id}", project);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("CreateProject")
            .WithSummary("Créer un nouveau projet")
            .WithDescription("Crée un nouveau projet avec les informations fournies")
            .RequireAuthorization("RequireTeacherRole")
            .Produces<ProjectResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

            group.MapGet("/", async ([FromServices] ProjectController controller) =>
            {
                try
                {
                    var projects = await controller.GetAllProjects();
                    return Results.Ok(projects);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("GetAllProjects")
            .WithSummary("Récupérer tous les projets")
            .WithDescription("Retourne la liste de tous les projets disponibles")
            .Produces<List<ProjectResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapGet("/{id}", async (int id, [FromServices] ProjectController controller) =>
            {
                try
                {
                    var project = await controller.GetProjectById(id);
                    return project is null ? Results.NotFound(new { message = "Projet non trouvé" }) : Results.Ok(project);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("GetProjectById")
            .WithSummary("Récupérer un projet par son ID")
            .WithDescription("Retourne les détails d'un projet spécifique")
            .Produces<ProjectResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPut("/{id}", async (int id, UpdateProjectDto dto, [FromServices] ProjectController controller, HttpContext context) =>
            {
                try
                {
                    var teacherId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(teacherId))
                    {
                        return Results.Unauthorized();
                    }
                    var project = await controller.UpdateProject(id, dto, teacherId);
                    return project is null ? Results.NotFound(new { message = "Projet non trouvé" }) : Results.Ok(project);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("UpdateProject")
            .WithSummary("Mettre à jour un projet")
            .WithDescription("Met à jour les informations d'un projet existant")
            .RequireAuthorization("RequireTeacherRole")
            .Produces<ProjectResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id}", async (int id, [FromServices] ProjectController controller, HttpContext context) =>
            {
                try
                {
                    var teacherId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(teacherId))
                    {
                        return Results.Unauthorized();
                    }
                    var result = await controller.DeleteProject(id, teacherId);
                    return result ? Results.NoContent() : Results.NotFound(new { message = "Projet non trouvé" });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("DeleteProject")
            .WithSummary("Supprimer un projet")
            .WithDescription("Supprime un projet existant")
            .RequireAuthorization("RequireTeacherRole")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
} 