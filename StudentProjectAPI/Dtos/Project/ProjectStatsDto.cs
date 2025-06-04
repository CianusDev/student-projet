using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Project
{
    // DTO pour les statistiques d'un projet
    public class ProjectStatsDto
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public int TotalAssignments { get; set; }
        public int SubmittedAssignments { get; set; }
        public int GradedAssignments { get; set; }
        public double AverageGrade { get; set; }
        public int TotalSubmissions { get; set; }
        public DateTime? LastSubmissionDate { get; set; }
        public List<DeliverableStatsDto> DeliverableStats { get; set; } = new();
    }
}