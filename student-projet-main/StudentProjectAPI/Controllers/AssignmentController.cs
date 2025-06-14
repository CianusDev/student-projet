using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.Project;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private static readonly List<object> Assignments = new();

        // POST: api/assignment
        [HttpPost]
        public IActionResult AssignProject([FromBody] AssignProjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.StudentIds != null && dto.StudentIds.Any())
            {
                foreach (var studentId in dto.StudentIds)
                {
                    Assignments.Add(new
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = dto.ProjectId,
                        UserId = studentId,
                        AssignedTo = "Student",
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            if (dto.GroupIds != null && dto.GroupIds.Any())
            {
                foreach (var groupId in dto.GroupIds)
                {
                    Assignments.Add(new
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = dto.ProjectId,
                        GroupId = groupId,
                        AssignedTo = "Group",
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            return Ok(new
            {
                message = "Projet assigné avec succès",
                assignments = Assignments
            });
        }

        // GET: api/assignment
        [HttpGet]
        public IActionResult GetAllAssignments()
        {
            return Ok(Assignments);
        }
    }
}
