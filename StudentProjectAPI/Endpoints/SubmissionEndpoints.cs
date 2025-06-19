using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Services;
using StudentProjectAPI.Dtos.Submission;
using StudentProjectAPI.DTOs.Submission;

namespace StudentProjectAPI.Endpoints
{
    public static class SubmissionEndpoints
    {
        public static void MapSubmissionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/submission")
                .WithTags("Soumissions");

            // Créer une soumission
            group.MapPost("/", async ([FromServices] ISubmissionService service, HttpContext http, [FromForm] CreateSubmissionWithFileDto dto) =>
            {
                var userId = http.User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type.EndsWith("nameidentifier"))?.Value;
                if (userId == null)
                    return Results.Unauthorized();
                var result = await service.CreateSubmissionAsync(
                    new CreateSubmissionDto
                    {
                        AssignmentId = dto.AssignmentId,
                        DeliverableId = dto.DeliverableId,
                        Comments = dto.Comments
                    },
                    userId,
                    dto.File
                );
                return Results.Ok(result);
            })
            .RequireAuthorization("Student")
            .WithName("CreateSubmission")
            .WithSummary("Créer une nouvelle soumission de livrable")
            .WithDescription("Permet à un étudiant de soumettre un livrable pour une assignation donnée. Un fichier doit être envoyé via un formulaire multipart.")
            .Accepts<CreateSubmissionWithFileDto>("multipart/form-data")
            .Produces<SubmissionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            // Récupérer une soumission par ID
            group.MapGet("/{id:int}", async ([FromServices] ISubmissionService service, int id) =>
            {
                var result = await service.GetSubmissionByIdAsync(id);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .RequireAuthorization()
            .WithName("GetSubmissionById")
            .WithSummary("Récupérer une soumission par son identifiant")
            .WithDescription("Retourne les détails d'une soumission spécifique selon son identifiant.")
            .Produces<SubmissionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

            // Liste des soumissions pour une assignation
            group.MapGet("/assignment/{assignmentId:int}", async ([FromServices] ISubmissionService service, int assignmentId) =>
            {
                var result = await service.GetSubmissionsByAssignmentAsync(assignmentId);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetSubmissionsByAssignment")
            .WithSummary("Lister les soumissions d'une assignation")
            .WithDescription("Retourne la liste de toutes les soumissions associées à une assignation donnée.")
            .Produces<List<SubmissionSummaryDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            // Télécharger un fichier de soumission
            group.MapGet("/download/{submissionId:int}", async ([FromServices] ISubmissionService service, int submissionId) =>
            {
                var fileDto = await service.DownloadSubmissionFileAsync(submissionId);
                if (fileDto == null) return Results.NotFound();
                return Results.File(fileDto.FileContent, fileDto.ContentType, fileDto.FileName);
            })
            .RequireAuthorization()
            .WithName("DownloadSubmissionFile")
            .WithSummary("Télécharger le fichier d'une soumission")
            .WithDescription("Permet de télécharger le fichier associé à une soumission spécifique.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
} 