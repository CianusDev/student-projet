using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    public class ProjectController(IProjectService projectService)
    {
        private readonly IProjectService _projectService = projectService;

        public async Task<IEnumerable<ProjectDto>> GetAllProjects()
        {
            return await _projectService.GetAllProjectsAsync();
        }

        public async Task<ProjectDto?> GetProjectById(int id)
        {
            return await _projectService.GetProjectByIdAsync(id);
        }

        public async Task<ProjectDto> CreateProject(CreateProjectDto dto, string teacherId)
        {
            return await _projectService.CreateProjectAsync(dto, teacherId);
        }

        public async Task<ProjectDto?> UpdateProject(int id, UpdateProjectDto dto, string teacherId)
        {
            return await _projectService.UpdateProjectAsync(id, dto, teacherId);
        }

        public async Task<bool> DeleteProject(int id, string teacherId)
        {
            return await _projectService.DeleteProjectAsync(id, teacherId);
        }
    }
} 