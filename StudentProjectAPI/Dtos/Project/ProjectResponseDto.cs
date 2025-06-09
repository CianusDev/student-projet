namespace StudentProjectAPI.Dtos.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int MaxPoints { get; set; }
        public bool IsGroupProject { get; set; }
        public int? MaxGroupSize { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
} 