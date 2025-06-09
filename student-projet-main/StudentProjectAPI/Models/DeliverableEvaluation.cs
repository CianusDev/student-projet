using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class DeliverableEvaluation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int EvaluationId { get; set; }
        
        [Required]
        public int SubmissionId { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Grade { get; set; }
        
        public string? Comments { get; set; }
        
        // Propriétés de navigation
        [ForeignKey("EvaluationId")]
        public virtual Evaluation Evaluation { get; set; } = null!;
        
        [ForeignKey("SubmissionId")]
        public virtual Submission Submission { get; set; } = null!;
    }
}