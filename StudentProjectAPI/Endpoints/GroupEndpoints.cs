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
                .WithTags("Groups");

            group.MapGet("/", async ([FromServices] GroupController controller) =>
            {
                var groups = await controller.GetGroups();
                return Results.Ok(groups);
            });

            group.MapGet("/{id}", async (int id, [FromServices] GroupController controller) =>
            {
                var group = await controller.GetGroup(id);
                return group is null ? Results.NotFound() : Results.Ok(group);
            });

            group.MapPost("/", async ([FromBody] CreateGroupDto createDto, [FromServices] GroupController controller) =>
            {
                var group = await controller.CreateGroup(createDto);
                return Results.Created($"/api/group/{group.Id}", group);
            });

            group.MapPut("/{id}", async (int id, [FromBody] UpdateGroupDto updateDto, [FromServices] GroupController controller) =>
            {
                var group = await controller.UpdateGroup(id, updateDto);
                return group is null ? Results.NotFound() : Results.Ok(group);
            });

            group.MapDelete("/{id}", async (int id, [FromServices] GroupController controller) =>
            {
                var deleted = await controller.DeleteGroup(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            group.MapPost("/{id}/members", async (int id, [FromBody] AddGroupMemberDto addMemberDto, [FromServices] GroupController controller) =>
            {
                var group = await controller.AddMember(id, addMemberDto);
                return group is null ? Results.NotFound() : Results.Ok(group);
            });

            group.MapDelete("/{id}/members/{memberId}", async (int id, int memberId, [FromServices] GroupController controller) =>
            {
                var removed = await controller.RemoveMember(id, memberId);
                return removed ? Results.NoContent() : Results.NotFound();
            });

            group.MapGet("/student/{studentId}", async (int studentId, [FromServices] GroupController controller) =>
            {
                var groups = await controller.GetStudentGroups(studentId);
                return Results.Ok(groups);
            });
        }
    }
} 