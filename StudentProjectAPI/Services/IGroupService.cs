using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
        Task<GroupDto?> GetGroupByIdAsync(int id);
        Task<GroupDto> CreateGroupAsync(CreateGroupDto createDto);
        Task<GroupDto?> UpdateGroupAsync(int id, UpdateGroupDto updateDto);
        Task<bool> DeleteGroupAsync(int id);
        Task<GroupDto?> AddMemberAsync(int groupId, AddGroupMemberDto addMemberDto);
        Task<bool> RemoveMemberAsync(int groupId, int memberId);
        Task<IEnumerable<StudentGroupDto>> GetStudentGroupsAsync(int studentId);
    }
} 