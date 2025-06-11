using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;

        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups
                .Include(g => g.Project)
                .Include(g => g.Members)
                    .ThenInclude(m => m.Student)
                .ToListAsync();

            return groups.Select(MapToGroupDto);
        }

        public async Task<GroupDto?> GetGroupByIdAsync(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Project)
                .Include(g => g.Members)
                    .ThenInclude(m => m.Student)
                .FirstOrDefaultAsync(g => g.Id == id);

            return group != null ? MapToGroupDto(group) : null;
        }

        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto createDto)
        {
            var group = new Group
            {
                ProjectId = createDto.ProjectId,
                GroupName = createDto.GroupName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            // Ajouter les membres
            foreach (var memberId in createDto.MemberIds)
            {
                var member = new GroupMember
                {
                    GroupId = group.Id,
                    StudentId = memberId,
                    IsLeader = memberId == createDto.LeaderId
                };
                _context.GroupMembers.Add(member);
            }

            await _context.SaveChangesAsync();

            return await GetGroupByIdAsync(group.Id) ?? throw new InvalidOperationException("Failed to create group");
        }

        public async Task<GroupDto?> UpdateGroupAsync(int id, UpdateGroupDto updateDto)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return null;

            if (updateDto.GroupName != null)
                group.GroupName = updateDto.GroupName;

            if (updateDto.LeaderId.HasValue)
            {
                var currentLeader = await _context.GroupMembers
                    .FirstOrDefaultAsync(m => m.GroupId == id && m.IsLeader);
                if (currentLeader != null)
                    currentLeader.IsLeader = false;

                var newLeader = await _context.GroupMembers
                    .FirstOrDefaultAsync(m => m.GroupId == id && m.StudentId == updateDto.LeaderId.Value);
                if (newLeader != null)
                    newLeader.IsLeader = true;
            }

            await _context.SaveChangesAsync();

            return await GetGroupByIdAsync(id);
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return false;

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GroupDto?> AddMemberAsync(int groupId, AddGroupMemberDto addMemberDto)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return null;

            var member = new GroupMember
            {
                GroupId = groupId,
                StudentId = addMemberDto.StudentId,
                IsLeader = addMemberDto.IsLeader
            };

            group.Members.Add(member);
            await _context.SaveChangesAsync();

            return await GetGroupByIdAsync(groupId);
        }

        public async Task<bool> RemoveMemberAsync(int groupId, int memberId)
        {
            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.StudentId == memberId);

            if (member == null) return false;

            _context.GroupMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentGroupDto>> GetStudentGroupsAsync(int studentId)
        {
            var groups = await _context.GroupMembers
                .Include(gm => gm.Group)
                    .ThenInclude(g => g.Project)
                .Where(gm => gm.StudentId == studentId)
                .Select(gm => new StudentGroupDto
                {
                    GroupId = gm.GroupId,
                    GroupName = gm.Group.GroupName,
                    ProjectId = gm.Group.ProjectId,
                    ProjectTitle = gm.Group.Project.Title,
                    IsLeader = gm.IsLeader,
                    JoinedAt = gm.JoinedAt,
                    MemberCount = gm.Group.Members.Count
                })
                .ToListAsync();

            return groups;
        }

        private static GroupDto MapToGroupDto(Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                ProjectId = group.ProjectId,
                ProjectTitle = group.Project.Title,
                GroupName = group.GroupName,
                CreatedAt = group.CreatedAt,
                Members = group.Members.Select(m => new GroupMemberDto
                {
                    Id = m.Id,
                    GroupId = m.GroupId,
                    StudentId = m.StudentId,
                    Student = new UserDto
                    {
                        Id = m.Student.Id,
                        Email = m.Student.Email,
                        FirstName = m.Student.FirstName,
                        LastName = m.Student.LastName,
                        Role = m.Student.Role,
                        CreatedAt = m.Student.CreatedAt
                    },
                    IsLeader = m.IsLeader,
                    JoinedAt = m.JoinedAt
                }).ToList()
            };
        }
    }
} 