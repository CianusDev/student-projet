using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public int TeacherId { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        public int MaxPoints { get; set; } = 20;
        
        public bool IsGroupProject { get; set; } = false;
        
        public int MaxGroupSize { get; set; } = 4;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; } = null!;
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<ProjectAssignment> Assignments { get; set; } = new List<ProjectAssignment>();
        public virtual ICollection<ProjectDeliverable> Deliverables { get; set; } = new List<ProjectDeliverable>();
    }
}