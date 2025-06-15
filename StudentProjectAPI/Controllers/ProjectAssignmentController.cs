using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.Assignment;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectAssignmentController(IProjectAssignmentService assignmentService) : ControllerBase
{
    private readonly IProjectAssignmentService _assignmentService = assignmentService;

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<AssignmentListItemDto>>> GetAllAssignments()
    {
        var assignments = await _assignmentService.GetAllAssignmentsAsync();
        return Ok(assignments);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<AssignmentDto>> GetAssignmentById(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment == null)
            return NotFound();

        return Ok(assignment);
    }

    [HttpGet("student/{studentId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AssignmentListItemDto>>> GetStudentAssignments(int studentId)
    {
        var assignments = await _assignmentService.GetStudentAssignmentsAsync(studentId);
        return Ok(assignments);
    }

    [HttpGet("group/{groupId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AssignmentListItemDto>>> GetGroupAssignments(int groupId)
    {
        var assignments = await _assignmentService.GetGroupAssignmentsAsync(groupId);
        return Ok(assignments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<AssignmentDto>> CreateAssignment(CreateAssignmentDto createDto)
    {
        var assignment = await _assignmentService.CreateAssignmentAsync(createDto);
        return CreatedAtAction(nameof(GetAssignmentById), new { id = assignment.Id }, assignment);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<AssignmentStatusDto>> UpdateAssignmentStatus(int id, UpdateAssignmentStatusDto updateDto)
    {
        var assignment = await _assignmentService.UpdateAssignmentStatusAsync(id, updateDto);
        if (assignment == null)
            return NotFound();

        return Ok(assignment);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var result = await _assignmentService.DeleteAssignmentAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 