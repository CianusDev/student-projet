using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.Assignment;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public class ProjectAssignmentService : IProjectAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public ProjectAssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetAllAssignmentsAsync()
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
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

            return assignment != null ? MapToAssignmentDto(assignment) : null;
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetStudentAssignmentsAsync(int studentId)
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                .Where(a => a.StudentId == studentId || 
                           (a.Group != null && a.Group.Members.Any(m => m.StudentId == studentId)))
                .ToListAsync();

            return assignments.Select(MapToAssignmentListItemDto);
        }

        public async Task<IEnumerable<AssignmentListItemDto>> GetGroupAssignmentsAsync(int groupId)
        {
            var assignments = await _context.ProjectAssignments
                .Include(a => a.Project)
                .Include(a => a.Student)
                .Include(a => a.Group)
                .Where(a => a.GroupId == groupId)
                .ToListAsync();

            return assignments.Select(MapToAssignmentListItemDto);
        }

        public async Task<AssignmentCreatedDto> CreateAssignmentAsync(CreateAssignmentDto createDto)
        {
            if (!createDto.StudentId.HasValue && !createDto.GroupId.HasValue)
            {
                throw new ArgumentException("Au moins un étudiant ou un groupe doit être spécifié");
            }

            var assignment = new ProjectAssignment
            {
                ProjectId = createDto.ProjectId,
                StudentId = createDto.StudentId ?? 0, // Valeur par défaut temporaire
                GroupId = createDto.GroupId,
                Status = "Assigned",
                AssignedAt = DateTime.UtcNow
            };

            _context.ProjectAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            var project = await _context.Projects.FindAsync(assignment.ProjectId);
            var student = createDto.StudentId.HasValue ? await _context.Users.FindAsync(createDto.StudentId.Value) : null;
            var group = createDto.GroupId.HasValue ? await _context.Groups.FindAsync(createDto.GroupId.Value) : null;

            return new AssignmentCreatedDto
            {
                Id = assignment.Id,
                ProjectId = assignment.ProjectId,
                ProjectTitle = project?.Title ?? string.Empty,
                StudentId = createDto.StudentId ?? 0,
                StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
                GroupId = assignment.GroupId,
                GroupName = group?.GroupName,
                Status = assignment.Status,
                AssignedAt = assignment.AssignedAt
            };
        }

        public async Task<AssignmentStatusDto?> UpdateAssignmentStatusAsync(int id, UpdateAssignmentStatusDto updateDto)
        {
            var assignment = await _context.ProjectAssignments
                .Include(a => a.Submissions)
                .Include(a => a.Evaluations)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (assignment == null) return null;

            assignment.Status = updateDto.Status;
            await _context.SaveChangesAsync();

            return new AssignmentStatusDto
            {
                Id = assignment.Id,
                Status = assignment.Status,
                LastSubmissionDate = assignment.Submissions.Max(s => s.SubmittedAt),
                SubmissionCount = assignment.Submissions.Count,
                EvaluationCount = assignment.Evaluations.Count
            };
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            var assignment = await _context.ProjectAssignments.FindAsync(id);
            if (assignment == null) return false;

            _context.ProjectAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }

        private static AssignmentDto MapToAssignmentDto(ProjectAssignment assignment)
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
                    MaxPoints = assignment.Project.MaxPoints,
                    IsGroupProject = assignment.Project.IsGroupProject,
                    MaxGroupSize = assignment.Project.MaxGroupSize
                },
                StudentId = assignment.StudentId,
                Student = assignment.Student != null ? new Dtos.User.UserDto
                {
                    Id = assignment.Student.Id,
                    Email = assignment.Student.Email,
                    FirstName = assignment.Student.FirstName,
                    LastName = assignment.Student.LastName,
                    Role = assignment.Student.Role,
                    CreatedAt = assignment.Student.CreatedAt
                } : null,
                GroupId = assignment.GroupId,
                Group = assignment.Group != null ? new Dtos.Group.GroupDto
                {
                    Id = assignment.Group.Id,
                    ProjectId = assignment.Group.ProjectId,
                    ProjectTitle = assignment.Group.Project.Title,
                    GroupName = assignment.Group.GroupName,
                    CreatedAt = assignment.Group.CreatedAt,
                    Members = assignment.Group.Members.Select(m => new Dtos.Group.GroupMemberDto
                    {
                        Id = m.Id,
                        GroupId = m.GroupId,
                        StudentId = m.StudentId,
                        Student = new Dtos.User.UserDto
                        {
                            Id = m.Student.Id,
                            Email = m.Student.Email,
                            FirstName = m.Student.FirstName,
                            LastName = m.Student.LastName,
                            Role = m.Student.Role,
                            CreatedAt = m.Student.CreatedAt
                        },
                        IsLeader = m.IsLeader,
                        JoinedAt = m.JoinedAt
                    }).ToList()
                } : null,
                Status = assignment.Status,
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
                ProjectTitle = assignment.Project.Title,
                StudentName = $"{assignment.Student?.FirstName} {assignment.Student?.LastName}",
                GroupName = assignment.Group?.GroupName,
                Status = assignment.Status,
                AssignedAt = assignment.AssignedAt
            };
        }
    }
} 