using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.Dtos.Assignment;

public class AssignmentCreatedDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string Status { get; set; } = "Assigned";
    public DateTime AssignedAt { get; set; }
} 