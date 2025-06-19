using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Group
{
    // DTO pour membre de groupe
    public class GroupMemberDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}