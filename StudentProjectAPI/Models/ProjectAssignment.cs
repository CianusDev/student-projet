using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class ProjectAssignment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        public string? StudentId { get; set; }
        
        public int? GroupId { get; set; } // Pour projet de groupe
        
        [Required]
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Assigned;
        
        [Required]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; } = null!;
        
        [ForeignKey("StudentId")]
        public virtual User? Student { get; set; }
        
        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; }
        
        public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}