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
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string TeacherId { get; set; } = string.Empty;
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        public bool IsGroupProject { get; set; }
        
        public int? MaxGroupSize { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        [ForeignKey("TeacherId")]
        public User Teacher { get; set; } = null!;
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<ProjectAssignment> Assignments { get; set; } = new List<ProjectAssignment>();
        public virtual ICollection<ProjectDeliverable> Deliverables { get; set; } = new List<ProjectDeliverable>();
    }
}