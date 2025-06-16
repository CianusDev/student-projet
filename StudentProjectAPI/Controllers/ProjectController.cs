using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    public class ProjectController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjects()
        {
            return await _projectService.GetAllProjectsAsync();
        }

        public async Task<ProjectResponseDto?> GetProjectById(int id)
        {
            return await _projectService.GetProjectByIdAsync(id);
        }

        public async Task<ProjectResponseDto> CreateProject(CreateProjectDto dto, int userId)
        {
            return await _projectService.CreateProjectAsync(dto, userId);
        }

        public async Task<ProjectResponseDto?> UpdateProject(int id, UpdateProjectDto dto)
        {
            return await _projectService.UpdateProjectAsync(id, dto);
        }

        public async Task<bool> DeleteProject(int id)
        {
            return await _projectService.DeleteProjectAsync(id);
        }
    }
} 