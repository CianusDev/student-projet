using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Models;
using StudentProjectAPI.Dtos.Submission;
using StudentProjectAPI.Dtos.User;


namespace StudentProjectAPI.Services
{
    public interface ISubmissionService
    {
        Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto createDto, string studentId, IFormFile? file);
        Task<SubmissionDto?> GetSubmissionByIdAsync(int id);
        Task<IEnumerable<SubmissionSummaryDto>> GetSubmissionsByAssignmentAsync(int assignmentId);
        Task<FileDownloadDto?> DownloadSubmissionFileAsync(int submissionId);
        // Ajoute d'autres méthodes selon les besoins (update, delete, etc.)
    }

    public class SubmissionService : ISubmissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public SubmissionService(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto createDto, string studentId, IFormFile? file)
        {
            // Vérification de l'existence de l'assignation et du livrable
            var assignment = await _context.ProjectAssignments.Include(a => a.Project).FirstOrDefaultAsync(a => a.Id == createDto.AssignmentId);
            if (assignment == null) throw new ArgumentException("Assignation introuvable");
            var deliverable = await _context.ProjectDeliverables.Include(d => d.DeliverableType).FirstOrDefaultAsync(d => d.Id == createDto.DeliverableId);
            if (deliverable == null) throw new ArgumentException("Livrable introuvable");
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null) throw new ArgumentException("Étudiant introuvable");

            // Gestion du fichier
            string? fileName = null;
            string? filePath = null;
            long? fileSize = null;
            if (file != null)
            {
                fileName = Path.GetFileName(file.FileName);
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "submissions");
                Directory.CreateDirectory(uploadsFolder);
                filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{fileName}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                fileSize = file.Length;
            }

            // Création de la soumission
            var submission = new Submission
            {
                AssignmentId = createDto.AssignmentId,
                DeliverableId = createDto.DeliverableId,
                SubmittedByStudentId = studentId,
                FileName = fileName,
                FilePath = filePath,
                FileSize = fileSize,
                Comments = createDto.Comments,
                SubmittedAt = DateTime.UtcNow,
                Version = 1 // TODO: gérer les versions si besoin
            };
            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            return await GetSubmissionByIdAsync(submission.Id) ?? throw new Exception("Erreur lors de la création de la soumission");
        }

        public async Task<SubmissionDto?> GetSubmissionByIdAsync(int id)
        {
            var submission = await _context.Submissions
                .Include(s => s.Deliverable).ThenInclude(d => d.DeliverableType)
                .Include(s => s.SubmittedByStudent)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (submission == null) return null;
            return MapToSubmissionDto(submission);
        }

        public async Task<IEnumerable<SubmissionSummaryDto>> GetSubmissionsByAssignmentAsync(int assignmentId)
        {
            var submissions = await _context.Submissions
                .Include(s => s.Deliverable)
                .Include(s => s.SubmittedByStudent)
                .Where(s => s.AssignmentId == assignmentId)
                .ToListAsync();
            return submissions.Select(MapToSubmissionSummaryDto);
        }

        public async Task<FileDownloadDto?> DownloadSubmissionFileAsync(int submissionId)
        {
            var submission = await _context.Submissions.FirstOrDefaultAsync(s => s.Id == submissionId);
            if (submission == null || string.IsNullOrEmpty(submission.FilePath) || !File.Exists(submission.FilePath))
                return null;
            var fileContent = await File.ReadAllBytesAsync(submission.FilePath);
            var contentType = "application/octet-stream"; // Peut être amélioré selon l'extension
            return new FileDownloadDto
            {
                FileName = submission.FileName ?? "fichier",
                ContentType = contentType,
                FileContent = fileContent
            };
        }

        // Méthodes de mapping
        private SubmissionDto MapToSubmissionDto(Submission s)
        {
            return new SubmissionDto
            {
                Id = s.Id,
                AssignmentId = s.AssignmentId,
                DeliverableId = s.DeliverableId,
                DeliverableTitle = s.Deliverable?.Title ?? string.Empty,
                DeliverableTypeName = s.Deliverable?.DeliverableType?.Name ?? string.Empty,
                SubmittedByStudentId = int.TryParse(s.SubmittedByStudentId, out var sid) ? sid : 0,
                SubmittedByStudent = new UserDto
                {
                    Id = s.SubmittedByStudent.Id,
                    Email = s.SubmittedByStudent.Email ?? string.Empty,
                    FirstName = s.SubmittedByStudent.FirstName,
                    LastName = s.SubmittedByStudent.LastName,
                    CreatedAt = s.SubmittedByStudent.CreatedAt
                },
                FileName = s.FileName,
                FileSize = s.FileSize,
                Comments = s.Comments,
                SubmittedAt = s.SubmittedAt,
                Version = s.Version,
                IsLate = s.Deliverable?.DueDate.HasValue == true && s.SubmittedAt > s.Deliverable.DueDate.Value,
                Evaluation = null // À compléter si besoin
            };
        }

        private SubmissionSummaryDto MapToSubmissionSummaryDto(Submission s)
        {
            return new SubmissionSummaryDto
            {
                Id = s.Id,
                StudentName = s.SubmittedByStudent != null ? $"{s.SubmittedByStudent.FirstName} {s.SubmittedByStudent.LastName}" : string.Empty,
                GroupName = s.Assignment.Group?.Name,
                DeliverableTitle = s.Deliverable?.Title ?? string.Empty,
                FileName = s.FileName,
                SubmittedAt = s.SubmittedAt,
                Version = s.Version,
                IsLate = s.Deliverable?.DueDate.HasValue == true && s.SubmittedAt > s.Deliverable.DueDate.Value,
                IsEvaluated = s.Evaluations.Count > 0,
                Grade = s.Evaluations.FirstOrDefault()?.Grade
            };
        }
    }
} 