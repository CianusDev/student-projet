using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTOs.Submission
{
    public class CreateSubmissionWithFileDto
    {
        [Required]
        public int AssignmentId { get; set; }
        [Required]
        public int DeliverableId { get; set; }
        [StringLength(1000)]
        public string? Comments { get; set; }
        public IFormFile? File { get; set; }
    }
} 