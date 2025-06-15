using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty; // Teacher, Student
        

        public string Specialite { get; set; } = string.Empty;

        public string NiveauEtude { get; set; } = string.Empty;

        public string Departement { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Project> TeacherProjects { get; set; } = [];
        public virtual ICollection<GroupMember> GroupMemberships { get; set; } = [];
        public virtual ICollection<ProjectAssignment> IndividualAssignments { get; set; } = [];
        public virtual ICollection<Submission> Submissions { get; set; } = [];
        public virtual ICollection<Evaluation> Evaluations { get; set; } = [];
    }
}