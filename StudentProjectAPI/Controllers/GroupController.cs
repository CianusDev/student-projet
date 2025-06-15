using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    public class GroupController(IGroupService groupService)
    {
        private readonly IGroupService _groupService = groupService;

        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            return await _groupService.GetAllGroupsAsync();
        }

        public async Task<GroupDto?> GetGroup(int id)
        {
            return await _groupService.GetGroupByIdAsync(id);
        }

        public async Task<GroupDto> CreateGroup(CreateGroupDto createDto)
        {
            return await _groupService.CreateGroupAsync(createDto);
        }

        public async Task<GroupDto?> UpdateGroup(int id, UpdateGroupDto updateDto)
        {
            return await _groupService.UpdateGroupAsync(id, updateDto);
        }

        public async Task<bool> DeleteGroup(int id)
        {
            return await _groupService.DeleteGroupAsync(id);
        }

        public async Task<GroupDto?> AddMember(int id, AddGroupMemberDto addMemberDto)
        {
            return await _groupService.AddMemberAsync(id, addMemberDto);
        }

        public async Task<bool> RemoveMember(int id, int memberId)
        {
            return await _groupService.RemoveMemberAsync(id, memberId);
        }

        public async Task<IEnumerable<StudentGroupDto>> GetStudentGroups(int studentId)
        {
            return await _groupService.GetStudentGroupsAsync(studentId);
        }
    }
} 