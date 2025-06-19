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
        public string ProjectName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<GroupMemberDto> Members { get; set; } = new();
        public string? LeaderId => Members.FirstOrDefault(m => m.IsLeader)?.StudentId;
        public string? LeaderName => Members.FirstOrDefault(m => m.IsLeader)?.StudentName;
        public int MemberCount => Members.Count;
    }
}