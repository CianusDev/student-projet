using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Group
{
    // DTO pour les groupes d'un étudiant
    public class StudentGroupDto
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; }
        public int MemberCount { get; set; }
    }
}