using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
        Task<GroupDto?> GetGroupByIdAsync(int id);
        Task<GroupDto> CreateGroupAsync(CreateGroupDto createDto, string studentId);
        Task<GroupDto?> UpdateGroupAsync(int id, UpdateGroupDto updateDto, string studentId);
        Task<bool> DeleteGroupAsync(int id, string studentId);
        Task<bool> AddMemberAsync(int groupId, string studentId, string newMemberId);
        Task<bool> RemoveMemberAsync(int groupId, string studentId, string memberId);
        Task<IEnumerable<StudentGroupDto>> GetStudentGroupsAsync(string studentId);
    }

    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public GroupService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            if (group == null)
                return null;

            return new GroupDto
            {
                Id = group.Id,
                ProjectId = group.ProjectId,
                Name = group.Name,
                CreatedAt = group.CreatedAt,
                Members = group.Members.Select(m => new GroupMemberDto
                {
                    StudentId = m.StudentId.ToString(),
                    StudentName = $"{m.Student.FirstName} {m.Student.LastName}",
                    IsLeader = m.IsLeader,
                    JoinedAt = m.JoinedAt
                }).ToList()
            };
        }

        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto createDto, string studentId)
        {
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
                throw new InvalidOperationException("Étudiant non trouvé");

            var roles = await _userManager.GetRolesAsync(student);
            if (!roles.Contains("Student"))
                throw new InvalidOperationException("L'utilisateur n'est pas un étudiant");

            var project = await _context.Projects.FindAsync(createDto.ProjectId);
            if (project == null)
                throw new InvalidOperationException("Projet non trouvé");

            var group = new Group
            {
                Name = createDto.Name,
                ProjectId = createDto.ProjectId,
                Members = new List<GroupMember>
                {
                    new GroupMember { StudentId = studentId, IsLeader = true }
                }
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return MapToGroupDto(group);
        }

        public async Task<GroupDto?> UpdateGroupAsync(int id, UpdateGroupDto updateDto, string studentId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return null;

            var member = group.Members.FirstOrDefault(m => m.StudentId.ToString() == studentId);
            if (member == null || !member.IsLeader)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à modifier ce groupe");

            group.Name = updateDto.GroupName ?? group.Name;
            await _context.SaveChangesAsync();

            return await GetGroupByIdAsync(id);
        }

        public async Task<bool> DeleteGroupAsync(int id, string studentId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return false;

            var member = group.Members.FirstOrDefault(m => m.StudentId.ToString() == studentId);
            if (member == null || !member.IsLeader)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à supprimer ce groupe");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMemberAsync(int groupId, string studentId, string newMemberId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                return false;

            var member = group.Members.FirstOrDefault(m => m.StudentId == studentId);
            if (member == null || !member.IsLeader)
                throw new InvalidOperationException("Vous n'êtes pas autorisé à ajouter des membres");

            var newMember = await _userManager.FindByIdAsync(newMemberId);
            if (newMember == null)
                throw new InvalidOperationException("L'étudiant n'existe pas");

            if (group.Members.Count >= group.Project.MaxGroupSize)
                throw new InvalidOperationException("Le groupe a atteint sa taille maximale");

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                StudentId = newMemberId,
                IsLeader = false,
                JoinedAt = DateTime.UtcNow
            };

            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveMemberAsync(int groupId, string studentId, string memberId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                return false;

            var isLeader = group.Members.Any(m => m.StudentId.ToString() == studentId && m.IsLeader);
            if (!isLeader)
                throw new InvalidOperationException("Vous n'êtes pas le leader de ce groupe");

            var member = group.Members.FirstOrDefault(m => m.StudentId.ToString() == memberId);
            if (member == null)
                throw new InvalidOperationException("Membre non trouvé");

            if (member.IsLeader)
                throw new InvalidOperationException("Impossible de retirer le leader du groupe");

            group.Members.Remove(member);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StudentGroupDto>> GetStudentGroupsAsync(string studentId)
        {
            var groups = await _context.GroupMembers
                .Include(gm => gm.Group)
                    .ThenInclude(g => g.Project)
                .Where(gm => gm.StudentId.ToString() == studentId)
                .Select(gm => new StudentGroupDto
                {
                    GroupId = gm.GroupId,
                    Name = gm.Group.Name,
                    ProjectId = gm.Group.ProjectId,
                    ProjectName = gm.Group.Project.Title,
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
                Name = group.Name,
                ProjectId = group.ProjectId,
                ProjectName = group.Project?.Title ?? string.Empty,
                Members = group.Members?.Select(m => new GroupMemberDto
                {
                    StudentId = m.StudentId.ToString(),
                    StudentName = $"{m.Student?.FirstName} {m.Student?.LastName}",
                    IsLeader = m.IsLeader,
                    JoinedAt = m.JoinedAt
                }).ToList() ?? new List<GroupMemberDto>()
            };
        }
    }
} 