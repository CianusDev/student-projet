using StudentProjectAPI.Dtos.Project;

namespace StudentProjectAPI.Services
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, int userId);
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();
        Task<ProjectResponseDto?> UpdateProjectAsync(int id, UpdateProjectDto dto);
        Task<bool> DeleteProjectAsync(int id);
    }
} 