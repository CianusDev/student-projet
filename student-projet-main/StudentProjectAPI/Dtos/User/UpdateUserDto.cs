using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.User
{
    public class UpdateUserDto
    {
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        public string? Email { get; set; }
    }
}