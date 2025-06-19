using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StudentProjectAPI.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Specialite { get; set; } = string.Empty;

        [StringLength(50)]
        public string NiveauEtude { get; set; } = string.Empty;

        [StringLength(100)]
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