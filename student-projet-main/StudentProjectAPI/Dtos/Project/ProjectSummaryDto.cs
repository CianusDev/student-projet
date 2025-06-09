using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Project
{
    // DTO pour liste des projets
    public class ProjectSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsGroupProject { get; set; }
        public int MaxGroupSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public string Status => DateTime.UtcNow > DueDate ? "Expired" : "Active";
    }
}