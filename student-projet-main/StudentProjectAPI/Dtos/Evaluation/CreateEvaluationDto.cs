using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTOs.Evaluation
{
    // DTO pour créer une évaluation
    public class CreateEvaluationDto
    {
        [Required(ErrorMessage = "L'ID de l'assignation est requis")]
        public int AssignmentId { get; set; }

        [Range(0, 20, ErrorMessage = "La note globale doit être entre 0 et 20")]
        public decimal? OverallGrade { get; set; }

        [StringLength(2000, ErrorMessage = "Les commentaires ne peuvent pas dépasser 2000 caractères")]
        public string? OverallComments { get; set; }

        public List<CreateDeliverableEvaluationDto> DeliverableEvaluations { get; set; } = new();
    }
}