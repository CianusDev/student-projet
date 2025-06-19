using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Dtos.Assignment
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public ProjectDto Project { get; set; } = null!;
        public string? StudentId { get; set; }
        public UserDto? Student { get; set; }
        public int? GroupId { get; set; }
        public GroupDto? Group { get; set; }
        public string Status { get; set; } = AssignmentStatus.Assigned.ToString();
        public DateTime AssignedAt { get; set; }
        public DateTime? LastSubmissionDate { get; set; }
        public int SubmissionCount { get; set; }
        public int EvaluationCount { get; set; }
    }
} 