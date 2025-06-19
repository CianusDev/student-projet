
namespace StudentProjectAPI.Dtos.Submission
{

    // DTO pour liste des soumissions
    public class SubmissionSummaryDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? GroupName { get; set; }
        public string DeliverableTitle { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int Version { get; set; }
        public bool IsLate { get; set; }
        public bool IsEvaluated { get; set; }
        public decimal? Grade { get; set; }
    }
}