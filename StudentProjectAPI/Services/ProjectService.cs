using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace StudentProjectAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Teacher)
                .ToListAsync();

            return projects.Select(MapToResponseDto);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project != null ? MapToResponseDto(project) : null;
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto, int userId)
        {
            var teacher = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == "Teacher")
                ?? throw new Exception("Enseignant non trouvé.");

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = dto.DueDate,
                MaxPoints = dto.MaxPoints,
                IsGroupProject = dto.IsGroupProject,
                MaxGroupSize = dto.MaxGroupSize,
                TeacherId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return MapToResponseDto(project);
        }

        public async Task<ProjectResponseDto?> UpdateProjectAsync(int id, UpdateProjectDto dto)
        {
            var project = await _context.Projects
                .Include(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userRole != "Teacher")
                throw new UnauthorizedAccessException("Seuls les enseignants peuvent modifier un projet.");

            // Mise à jour conditionnelle
            if (!string.IsNullOrWhiteSpace(dto.Title))
                project.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                project.Description = dto.Description;

            if (dto.DueDate.HasValue)
                project.DueDate = dto.DueDate.Value;

            if (dto.MaxPoints.HasValue)
                project.MaxPoints = dto.MaxPoints.Value;

            if (dto.IsGroupProject.HasValue)
                project.IsGroupProject = dto.IsGroupProject.Value;

            if (dto.MaxGroupSize.HasValue)
                project.MaxGroupSize = dto.MaxGroupSize.Value;

            await _context.SaveChangesAsync();

            return MapToResponseDto(project);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return false;

            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userRole != "Teacher")
                throw new UnauthorizedAccessException("Seuls les enseignants peuvent supprimer un projet.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }

        private static ProjectResponseDto MapToResponseDto(Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description ?? string.Empty,
                CreatedAt = project.CreatedAt,
                DueDate = project.DueDate,
                MaxPoints = project.MaxPoints,
                IsGroupProject = project.IsGroupProject,
                MaxGroupSize = project.MaxGroupSize,
                CreatedBy = $"{project.Teacher.FirstName} {project.Teacher.LastName}",
                Status = "Active" // Peut être personnalisé selon logique métier
            };
        }
    }
}
