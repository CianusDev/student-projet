using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
// using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Project
{
   // DTO pour créer un projet
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "La date d'échéance est requise")]
        public DateTime DueDate { get; set; }

        [Range(0, 1000, ErrorMessage = "Les points doivent être entre 0 et 1000")]
        public int MaxPoints { get; set; } = 100;

        public bool IsGroupProject { get; set; } = false;

        [Range(1, 10, ErrorMessage = "La taille du groupe doit être entre 1 et 10")]
        public int MaxGroupSize { get; set; } = 4;

        public List<CreateProjectDeliverableDto> Deliverables { get; set; } = new();
    }

}