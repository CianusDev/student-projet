using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Dtos.Evaluation;

namespace StudentProjectAPI.Dtos.Submission
{

    // DTO pour afficher une soumission
    public class SubmissionDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int DeliverableId { get; set; }
        public string DeliverableTitle { get; set; } = string.Empty;
        public string DeliverableTypeName { get; set; } = string.Empty;
        public int SubmittedByStudentId { get; set; }
        public UserDto SubmittedByStudent { get; set; } = null!;
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public string? FileSizeFormatted => FileSize.HasValue ? FormatFileSize(FileSize.Value) : null;
        public string? Comments { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int Version { get; set; }
        public bool IsLate { get; set; }
        public EvaluationDto? Evaluation { get; set; }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}