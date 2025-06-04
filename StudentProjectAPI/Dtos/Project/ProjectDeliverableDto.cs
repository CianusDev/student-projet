using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Project
{
    public class ProjectDeliverableDto
    {
        public int Id { get; set; }
        public int DeliverableTypeId { get; set; }
        public string DeliverableTypeName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsRequired { get; set; }
        public int MaxPoints { get; set; }
        public string? AllowedExtensions { get; set; }
        public int MaxFileSize { get; set; }
    }
}