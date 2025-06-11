using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class DeliverableType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? AllowedExtensions { get; set; }
        
        public int MaxFileSize { get; set; } = 10; // en MB
        
        // Navigation properties
        public virtual ICollection<ProjectDeliverable> ProjectDeliverables { get; set; } = new List<ProjectDeliverable>();
    }
}