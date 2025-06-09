using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
// using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Project
{
   // DTO pour créer un projet
    // DTO pour modifier un projet
    public class UpdateProjectDto
    {
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string? Title { get; set; }

        [StringLength(2000, ErrorMessage = "La description ne peut pas dépasser 2000 caractères")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }

        [Range(1, 1000, ErrorMessage = "Les points maximum doivent être entre 1 et 1000")]
        public int? MaxPoints { get; set; }

        [Range(2, 10, ErrorMessage = "La taille maximum du groupe doit être entre 2 et 10")]
        public int? MaxGroupSize { get; set; }
    }
    
}