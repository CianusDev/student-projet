using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Assignment
{
    public class UpdateAssignmentStatusDto
    {
        [Required(ErrorMessage = "Le statut est requis")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Assigned, InProgress, Submitted, Graded
    }
} 