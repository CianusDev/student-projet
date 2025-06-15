using StudentProjectAPI.Dtos.Assignment;

namespace StudentProjectAPI.Services
{
    public interface IProjectAssignmentService
    {
        Task<IEnumerable<AssignmentListItemDto>> GetAllAssignmentsAsync();
        Task<AssignmentDto?> GetAssignmentByIdAsync(int id);
        Task<IEnumerable<AssignmentListItemDto>> GetStudentAssignmentsAsync(int studentId);
        Task<IEnumerable<AssignmentListItemDto>> GetGroupAssignmentsAsync(int groupId);
        Task<AssignmentCreatedDto> CreateAssignmentAsync(CreateAssignmentDto createDto);
        Task<AssignmentStatusDto?> UpdateAssignmentStatusAsync(int id, UpdateAssignmentStatusDto updateDto);
        Task<bool> DeleteAssignmentAsync(int id);
    }
} 