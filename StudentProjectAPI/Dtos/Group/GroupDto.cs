using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Group
{
    // DTO pour afficher un groupe
    public class GroupDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<GroupMemberDto> Members { get; set; } = new();
        public UserDto? Leader => Members.FirstOrDefault(m => m.IsLeader)?.Student;
        public int MemberCount => Members.Count;
    }
}