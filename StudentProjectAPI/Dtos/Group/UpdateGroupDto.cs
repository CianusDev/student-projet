using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Group
{
    // DTO pour modifier un groupe
    public class UpdateGroupDto
    {
        [StringLength(100, ErrorMessage = "Le nom du groupe ne peut pas dépasser 100 caractères")]
        public string? GroupName { get; set; }

        public int? LeaderId { get; set; }
    }

}