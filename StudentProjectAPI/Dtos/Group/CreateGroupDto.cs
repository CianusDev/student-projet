using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Group
{
    // DTO pour créer un groupe
    public class CreateGroupDto
    {
        [Required(ErrorMessage = "L'ID du projet est requis")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Le nom du groupe est requis")]
        [StringLength(100, ErrorMessage = "Le nom du groupe ne peut pas dépasser 100 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Au moins un membre est requis")]
        public List<int> MemberIds { get; set; } = new();

        public int? LeaderId { get; set; }
    }
}