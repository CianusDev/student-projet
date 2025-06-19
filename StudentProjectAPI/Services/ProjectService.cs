using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Models;
using Microsoft.AspNetCore.Http;

namespace StudentProjectAPI.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createDto, string teacherId);
        Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateDto, string teacherId);
        Task<bool> DeleteProjectAsync(int id, string teacherId);
    }

    public class ProjectService(ApplicationDbContext context, UserManager<User> userManager) : IProjectService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Teacher)
                .Include(p => p.Groups)
                .ToListAsync();

            return projects.Select(MapToProjectDto);
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Teacher)
                .Include(p => p.Groups)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project != null ? MapToProjectDto(project) : null;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createDto, string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);
            if (teacher == null)
            {
                throw new KeyNotFoundException("Teacher not found");
            }

            var project = new Project
            {
                Title = createDto.Title ?? string.Empty,
                Description = createDto.Description ?? string.Empty,
                TeacherId = teacherId,
                DueDate = createDto.DueDate,
                IsGroupProject = createDto.IsGroupProject,
                MaxGroupSize = createDto.MaxGroupSize
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return MapToProjectDto(project);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateDto, string teacherId)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return null;
            }

            if (project.TeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this project");
            }

            project.Title = updateDto.Title ?? project.Title;
            project.Description = updateDto.Description ?? project.Description;
            project.DueDate = updateDto.DueDate ?? project.DueDate;
            project.IsGroupProject = updateDto.IsGroupProject ?? project.IsGroupProject;
            project.MaxGroupSize = updateDto.MaxGroupSize;

            await _context.SaveChangesAsync();

            return MapToProjectDto(project);
        }

        public async Task<bool> DeleteProjectAsync(int id, string teacherId)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            if (project.TeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this project");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }

        private static ProjectDto MapToProjectDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                DueDate = project.DueDate,
                MaxGroupSize = project.MaxGroupSize ?? 0,
                TeacherId = project.TeacherId,
                TeacherName = $"{project.Teacher?.FirstName} {project.Teacher?.LastName}",
                Groups = [.. project.Groups.Select(g => new Dtos.Group.GroupDto
                {
                    Id = g.Id,
                    ProjectId = g.ProjectId,
                    Name = g.Name,
                    CreatedAt = g.CreatedAt
                })]
            };
        }
    }
} 