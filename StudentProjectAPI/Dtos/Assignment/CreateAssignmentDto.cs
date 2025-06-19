using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Assignment
{
    public class CreateAssignmentDto
    {
        [Required(ErrorMessage = "L'ID du projet est requis")]
        public int ProjectId { get; set; }

        public string? StudentId { get; set; } // Pour projet individuel
        public int? GroupId { get; set; } // Pour projet de groupe
    }
} 