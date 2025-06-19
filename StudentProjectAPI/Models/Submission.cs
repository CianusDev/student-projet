using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int AssignmentId { get; set; }
        
        [Required]
        public int DeliverableId { get; set; }
        
        [Required]
        public string SubmittedByStudentId { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? FileName { get; set; }
        
        [StringLength(500)]
        public string? FilePath { get; set; }
        
        public long? FileSize { get; set; }
        
        public string? Comments { get; set; }
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        public int Version { get; set; } = 1;
        
        // Propriétés de navigation
        [ForeignKey("AssignmentId")]
        public virtual ProjectAssignment Assignment { get; set; } = null!;
        
        [ForeignKey("DeliverableId")]
        public virtual ProjectDeliverable Deliverable { get; set; } = null!;
        
        [ForeignKey("SubmittedByStudentId")]
        public virtual User SubmittedByStudent { get; set; } = null!;
        
        public virtual ICollection<DeliverableEvaluation> Evaluations { get; set; } = [];
    }
}