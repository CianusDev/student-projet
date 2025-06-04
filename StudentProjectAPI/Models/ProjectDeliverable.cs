using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class ProjectDeliverable
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int DeliverableTypeId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public bool IsRequired { get; set; } = true;
        
        public int MaxPoints { get; set; } = 0;
        
        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; } = null!;
        
        [ForeignKey("DeliverableTypeId")]
        public virtual DeliverableType DeliverableType { get; set; } = null!;
        
        public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public virtual ICollection<DeliverableEvaluation> Evaluations { get; set; } = new List<DeliverableEvaluation>();
    }
}