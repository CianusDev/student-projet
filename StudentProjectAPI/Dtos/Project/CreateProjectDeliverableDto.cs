using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Project
{
    public class CreateProjectDeliverableDto
    {
        [Required(ErrorMessage = "Le type de livrable est requis")]
        public int DeliverableTypeId { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }

        public bool IsRequired { get; set; } = true;

        [Range(0, 1000, ErrorMessage = "Les points maximum doivent être entre 0 et 1000")]
        public int MaxPoints { get; set; } = 0;
    }
}