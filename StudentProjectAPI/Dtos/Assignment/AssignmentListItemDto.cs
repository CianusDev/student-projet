namespace StudentProjectAPI.Dtos.Assignment;

public class AssignmentListItemDto
{
    public int Id { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string? GroupName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
} 