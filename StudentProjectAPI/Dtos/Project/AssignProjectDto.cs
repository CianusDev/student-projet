using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Project
{
    // DTO pour assigner un projet
    public class AssignProjectDto
    {
        [Required(ErrorMessage = "L'ID du projet est requis")]
        public int ProjectId { get; set; }

        // Pour projet individuel
        public List<int> StudentIds { get; set; } = new();

        // Pour projet de groupe
        public List<int> GroupIds { get; set; } = new();
    }
}