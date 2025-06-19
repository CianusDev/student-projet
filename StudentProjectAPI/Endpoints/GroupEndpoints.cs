using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Controllers;
using StudentProjectAPI.Dtos.Group;

namespace StudentProjectAPI.Endpoints
{
    public static class GroupEndpoints
    {
        public static void MapGroupEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/group")
                .WithTags("Groupes");

            group.MapGet("/", async ([FromServices] GroupController controller) =>
            {
                var groups = await controller.GetGroups();
                return Results.Ok(groups);
            })
            .WithName("GetAllGroups")
            .WithSummary("Récupérer tous les groupes")
            .WithDescription("Retourne la liste de tous les groupes existants.")
            .Produces<List<GroupDto>>(StatusCodes.Status200OK);

            group.MapGet("/{id}", async (int id, [FromServices] GroupController controller) =>
            {
                var groupResult = await controller.GetGroup(id);
                return groupResult is null ? Results.NotFound() : Results.Ok(groupResult);
            })
            .WithName("GetGroupById")
            .WithSummary("Récupérer un groupe par son ID")
            .WithDescription("Retourne les informations d'un groupe spécifique selon son identifiant.")
            .Produces<GroupDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async ([FromBody] CreateGroupDto createDto, [FromQuery] string studentId, [FromServices] GroupController controller) =>
            {
                var result = await controller.CreateGroup(createDto, studentId);
                return Results.Created($"/api/group/{result.Value?.Id}", result.Value);
            })
            .WithName("CreateGroup")
            .WithSummary("Créer un nouveau groupe")
            .WithDescription("Crée un nouveau groupe de projet et retourne le groupe créé.")
            .Produces<GroupDto>(StatusCodes.Status201Created);

            group.MapPut("/{id}", async (int id, [FromBody] UpdateGroupDto updateDto, [FromQuery] string studentId, [FromServices] GroupController controller) =>
            {
                var groupResult = await controller.UpdateGroup(id, updateDto, studentId);
                return groupResult is null ? Results.NotFound() : Results.Ok(groupResult);
            })
            .WithName("UpdateGroup")
            .WithSummary("Mettre à jour un groupe")
            .WithDescription("Met à jour les informations d'un groupe existant.")
            .Produces<GroupDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id}", async (int id, [FromQuery] string studentId, [FromServices] GroupController controller) =>
            {
                var result = await controller.DeleteGroup(id, studentId);
                return result is NoContentResult ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGroup")
            .WithSummary("Supprimer un groupe")
            .WithDescription("Supprime un groupe existant selon son identifiant.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/{id}/members", async (int id, [FromQuery] string studentId, [FromQuery] string newMemberId, [FromServices] GroupController controller) =>
            {
                var result = await controller.AddMember(id, studentId, newMemberId);
                return result is OkResult ? Results.Ok() : Results.NotFound();
            })
            .WithName("AddGroupMember")
            .WithSummary("Ajouter un membre à un groupe")
            .WithDescription("Ajoute un nouvel étudiant à un groupe existant.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id}/members", async (int id, [FromQuery] string studentId, [FromQuery] string memberId, [FromServices] GroupController controller) =>
            {
                var result = await controller.RemoveMember(id, studentId, memberId);
                return result is OkResult ? Results.Ok() : Results.NotFound();
            })
            .WithName("RemoveGroupMember")
            .WithSummary("Supprimer un membre d'un groupe")
            .WithDescription("Supprime un membre d'un groupe existant.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/student/{studentId}", async (string studentId, [FromServices] GroupController controller) =>
            {
                var groups = await controller.GetStudentGroups(studentId);
                return Results.Ok(groups);
            })
            .WithName("GetGroupsByStudent")
            .WithSummary("Récupérer les groupes d'un étudiant")
            .WithDescription("Retourne la liste des groupes auxquels appartient un étudiant donné.")
            .Produces<List<StudentGroupDto>>(StatusCodes.Status200OK);
        }
    }
} 