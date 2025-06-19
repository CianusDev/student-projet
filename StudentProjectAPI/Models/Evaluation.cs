using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int AssignmentId { get; set; }
        
        [Required]
        public string TeacherId { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? OverallGrade { get; set; }
        
        public string? OverallComments { get; set; }
        
        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("AssignmentId")]
        public virtual ProjectAssignment Assignment { get; set; } = null!;
        
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; } = null!;
        
        public virtual ICollection<DeliverableEvaluation> DeliverableEvaluations { get; set; } = new List<DeliverableEvaluation>();
    }
}