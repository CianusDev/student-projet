using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Project;
using StudentProjectAPI.Models;
using Microsoft.AspNetCore.Http;

namespace StudentProjectAPI.Services
{
    public class ProjectService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IProjectService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
                ?? throw new Exception("Enseignant non trouvé");

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

            // Vérifier que l'utilisateur est bien le créateur du projet
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (project.TeacherId != userId)
                throw new Exception("Vous n'êtes pas autorisé à modifier ce projet");

            // Mise à jour des propriétés avec vérification des valeurs nullables
            if (dto.Title != null)
                project.Title = dto.Title;
            
            if (dto.Description != null)
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
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;

            // Vérifier que l'utilisateur est bien le créateur du projet
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (project.TeacherId != userId)
                throw new Exception("Vous n'êtes pas autorisé à supprimer ce projet");

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
                Status = "Active" // À implémenter selon la logique métier
            };
        }
    }
} 