using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string StudentId { get; set; } = string.Empty;
        
        [Required]
        public int GroupId { get; set; }
        
        [Required]
        public bool IsLeader { get; set; }
        
        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; } = null!;
        
        [ForeignKey("StudentId")]
        public virtual User Student { get; set; } = null!;
    }
}