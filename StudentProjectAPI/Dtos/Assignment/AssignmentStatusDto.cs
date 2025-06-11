namespace StudentProjectAPI.Dtos.Assignment;

public class AssignmentStatusDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastSubmissionDate { get; set; }
    public int SubmissionCount { get; set; }
    public int EvaluationCount { get; set; }
} 