using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Project
{
    // DTO pour afficher un projet
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TeacherId { get; set; }
        public UserDto Teacher { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public int MaxPoints { get; set; }
        public bool IsGroupProject { get; set; }
        public int MaxGroupSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProjectDeliverableDto> Deliverables { get; set; } = new();
        public List<GroupDto> Groups { get; set; } = new();
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public string Status => DateTime.UtcNow > DueDate ? "Expired" : "Active";
    }
}