using Microsoft.AspNetCore.Authorization;
using StudentProjectAPI.Controllers;
using StudentProjectAPI.Dtos.Assignment;
using Microsoft.AspNetCore.Mvc;

namespace StudentProjectAPI.Endpoints;

public static class ProjectAssignmentEndpoints
{
    public static void MapProjectAssignmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/assignment")
            .WithTags("Project Assignments")
            .WithOpenApi();

        group.MapGet("/", async (ProjectAssignmentController controller) =>
        {
            var assignments = await controller.GetAllAssignments();
            return assignments;
        })
        .WithName("GetAllAssignments")
        .WithSummary("Get all project assignments")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Teacher"));

        group.MapGet("/{id}", async (int id, ProjectAssignmentController controller) =>
        {
            var assignment = await controller.GetAssignmentById(id);
            return assignment;
        })
        .WithName("GetAssignmentById")
        .WithSummary("Get a specific project assignment")
        .RequireAuthorization();

        group.MapGet("/student/{studentId}", async (int studentId, ProjectAssignmentController controller) =>
        {
            var assignments = await controller.GetStudentAssignments(studentId);
            return assignments;
        })
        .WithName("GetStudentAssignments")
        .WithSummary("Get all assignments for a specific student")
        .RequireAuthorization();

        group.MapGet("/group/{groupId}", async (int groupId, ProjectAssignmentController controller) =>
        {
            var assignments = await controller.GetGroupAssignments(groupId);
            return assignments;
        })
        .WithName("GetGroupAssignments")
        .WithSummary("Get all assignments for a specific group")
        .RequireAuthorization();

        group.MapPost("/", async (CreateAssignmentDto createDto, ProjectAssignmentController controller) =>
        {
            var result = await controller.CreateAssignment(createDto);
            return result;
        })
        .WithName("CreateAssignment")
        .WithSummary("Create a new project assignment")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Teacher"));

        group.MapPut("/{id}/status", async (int id, UpdateAssignmentStatusDto updateDto, ProjectAssignmentController controller) =>
        {
            var result = await controller.UpdateAssignmentStatus(id, updateDto);
            return result;
        })
        .WithName("UpdateAssignmentStatus")
        .WithSummary("Update the status of a project assignment")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Teacher"));

        group.MapDelete("/{id}", async (int id, ProjectAssignmentController controller) =>
        {
            var result = await controller.DeleteAssignment(id);
            return result switch
            {
                NoContentResult => Results.NoContent(),
                NotFoundResult => Results.NotFound(),
                _ => Results.StatusCode(500)
            };
        })
        .WithName("DeleteAssignment")
        .WithSummary("Delete a project assignment")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Teacher"));
    }
} 