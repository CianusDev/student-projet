using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTOs.Evaluation
{

    // DTO pour évaluation d'un livrable
    public class CreateDeliverableEvaluationDto
    {
        [Required(ErrorMessage = "L'ID de la soumission est requis")]
        public int SubmissionId { get; set; }

        [Range(0, 20, ErrorMessage = "La note doit être entre 0 et 20")]
        public decimal? Grade { get; set; }

        [StringLength(1000, ErrorMessage = "Les commentaires ne peuvent pas dépasser 1000 caractères")]
        public string? Comments { get; set; }
    }
}