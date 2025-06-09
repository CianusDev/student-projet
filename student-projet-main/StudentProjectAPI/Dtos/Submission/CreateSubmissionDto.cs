using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTOs.Submission
{
    // DTO pour soumettre un livrable
    public class CreateSubmissionDto
    {
        [Required(ErrorMessage = "L'ID de l'assignation est requis")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "L'ID du livrable est requis")]
        public int DeliverableId { get; set; }

        [StringLength(1000, ErrorMessage = "Les commentaires ne peuvent pas dépasser 1000 caractères")]
        public string? Comments { get; set; }

        // Le fichier sera géré séparément via IFormFile
    }
}