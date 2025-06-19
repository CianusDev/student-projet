using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Assignment;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public interface IProjectAssignmentService
    {
        Task<IEnumerable<AssignmentListItemDto>> GetAllAssignmentsAsync();
        Task<AssignmentDto?> GetAssignmentByIdAsync(int id);
        Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createDto, string teacherId);
        Task<AssignmentDto?> UpdateAssignmentStatusAsync(int id, UpdateAssignmentStatusDto updateDto, string teacherId);
        Task<bool> DeleteAssignmentAsync(int id, string teacherId);
        Task<IEnumerable<AssignmentListItemDto>> GetStudentAssignmentsAsync(string studentId);
        Task<IEnumerable<AssignmentListItemDto>> GetGroupAssignmentsAsync(int groupId);
    }

    public class ProjectAssignmentService : IProjectAssignmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProjectAssignmentService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetAllAssignmentsAsync()
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .ToListAsync();

            return assignments.Select(MapToAssignmentListItemDto);
        }

        public async Task<AssignmentDto?> GetAssignmentByIdAsync(int id)
        {
            var assignment = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                    .ThenInclude(g => g!.Members)
                        .ThenInclude(m => m.Student)
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .FirstOrDefaultAsync(a => a.Id == id);

            return assignment != null ? MapToProjectAssignmentDto(assignment) : null;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createDto, string teacherId)
        {
            if (string.IsNullOrEmpty(createDto.StudentId) && !createDto.GroupId.HasValue)
            {
                throw new ArgumentException("Au moins un étudiant ou un groupe doit être spécifié");
            }

            var project = await _context.Projects.FindAsync(createDto.ProjectId);
            if (project == null)
            {
                throw new ArgumentException("Le projet spécifié n'existe pas");
            }

            if (project.TeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("Vous n'êtes pas autorisé à assigner ce projet");
            }

            var assignment = new ProjectAssignment
            {
                ProjectId = createDto.ProjectId,
                StudentId = createDto.StudentId?.ToString(),
                GroupId = createDto.GroupId,
                Status = AssignmentStatus.Assigned,
                AssignedAt = DateTime.UtcNow
            };

            _context.ProjectAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            return await GetAssignmentByIdAsync(assignment.Id) ?? throw new InvalidOperationException("Failed to create assignment");
        }

        public async Task<AssignmentDto?> UpdateAssignmentStatusAsync(int id, UpdateAssignmentStatusDto updateDto, string teacherId)
        {
            var assignment = await _context.ProjectAssignments
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (assignment == null) return null;

            assignment.Status = (AssignmentStatus)Enum.Parse(typeof(AssignmentStatus), updateDto.Status);
            await _context.SaveChangesAsync();

            return await GetAssignmentByIdAsync(id);
        }

        public async Task<bool> DeleteAssignmentAsync(int id, string teacherId)
        {
            var assignment = await _context.ProjectAssignments.FindAsync(id);
            if (assignment == null) return false;

            _context.ProjectAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetStudentAssignmentsAsync(string studentId)
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            return assignments.Select(MapToAssignmentListItemDto);
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetGroupAssignmentsAsync(int groupId)
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .Where(a => a.GroupId == groupId)
                .ToListAsync();

            return assignments.Select(MapToAssignmentListItemDto);
        }

        private static AssignmentDto MapToProjectAssignmentDto(ProjectAssignment assignment)
        {
            return new AssignmentDto
            {
                Id = assignment.Id,
                ProjectId = assignment.ProjectId,
                Project = new Dtos.Project.ProjectDto
                {
                    Id = assignment.Project.Id,
                    Title = assignment.Project.Title,
                    Description = assignment.Project.Description,
                    CreatedAt = assignment.Project.CreatedAt,
                    DueDate = assignment.Project.DueDate,
                    IsGroupProject = assignment.Project.IsGroupProject,
                    MaxGroupSize = assignment.Project.MaxGroupSize ?? 0
                },
                StudentId = assignment.StudentId,
                Student = assignment.Student != null ? new Dtos.User.UserDto
                {
                    Id = assignment.Student.Id,
                    Email = assignment.Student.Email ?? string.Empty,
                    FirstName = assignment.Student.FirstName ?? string.Empty,
                    LastName = assignment.Student.LastName ?? string.Empty,
                    CreatedAt = assignment.Student.CreatedAt
                } : null,
                GroupId = assignment.GroupId,
                Group = assignment.Group != null ? new Dtos.Group.GroupDto
                {
                    Id = assignment.Group.Id,
                    ProjectId = assignment.Group.ProjectId,
                    Name = assignment.Group.Name,
                    CreatedAt = assignment.Group.CreatedAt
                } : null,
                Status = assignment.Status.ToString(),
                AssignedAt = assignment.AssignedAt,
                LastSubmissionDate = assignment.Submissions.Max(s => s.SubmittedAt),
                SubmissionCount = assignment.Submissions.Count,
                EvaluationCount = assignment.Evaluations.Count
            };
        }

        private static AssignmentListItemDto MapToAssignmentListItemDto(ProjectAssignment assignment)
        {
            return new AssignmentListItemDto
            {
                Id = assignment.Id,
                ProjectId = assignment.ProjectId,
                Project = new Dtos.Project.ProjectDto
                {
                    Id = assignment.Project.Id,
                    Title = assignment.Project.Title,
                    Description = assignment.Project.Description,
                    CreatedAt = assignment.Project.CreatedAt,
                    DueDate = assignment.Project.DueDate,
                    IsGroupProject = assignment.Project.IsGroupProject,
                    MaxGroupSize = assignment.Project.MaxGroupSize ?? 0
                },
                StudentId = assignment.StudentId,
                Student = assignment.Student != null ? new Dtos.User.UserDto
                {
                    Id = assignment.Student.Id,
                    Email = assignment.Student.Email ?? string.Empty,
                    FirstName = assignment.Student.FirstName ?? string.Empty,
                    LastName = assignment.Student.LastName ?? string.Empty,
                    CreatedAt = assignment.Student.CreatedAt
                } : null,
                GroupId = assignment.GroupId,
                GroupName = assignment.Group?.Name,
                Status = assignment.Status.ToString(),
                AssignedAt = assignment.AssignedAt,
                LastSubmissionDate = assignment.Submissions.Max(s => s.SubmittedAt),
                SubmissionCount = assignment.Submissions.Count,
                EvaluationCount = assignment.Evaluations.Count
            };
        }
    }
} 