using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Project
{
    // DTO pour afficher un projet
    public class ProjectDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string TeacherId { get; set; } = string.Empty;
        
        public string TeacherName { get; set; } = string.Empty;
        
        [Required]
        public DateTime DueDate { get; set; }
        
        public int MaxPoints { get; set; } = 100;
        
        public bool IsGroupProject { get; set; } = false;
        
        public int MaxGroupSize { get; set; } = 4;
        
        public DateTime CreatedAt { get; set; }
        public List<ProjectDeliverableDto> Deliverables { get; set; } = new();
        public List<GroupDto> Groups { get; set; } = new();
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public string Status => DateTime.UtcNow > DueDate ? "Expired" : "Active";
    }
}