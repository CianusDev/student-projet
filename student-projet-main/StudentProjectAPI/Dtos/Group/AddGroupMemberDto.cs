using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Group
{
    public class AddGroupMemberDto
    {
        [Required(ErrorMessage = "L'ID de l'étudiant est requis")]
        public int StudentId { get; set; }

        public bool IsLeader { get; set; } = false;
    }
}