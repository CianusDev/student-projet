using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Project
{
     // DTO pour les statistiques d'un livrable
    public class DeliverableStatsDto
    {
        public int DeliverableId { get; set; }
        public string DeliverableTitle { get; set; } = string.Empty;
        public int TotalExpected { get; set; }
        public int TotalSubmitted { get; set; }
        public double SubmissionRate => TotalExpected > 0 ? (double)TotalSubmitted / TotalExpected * 100 : 0;
        public double AverageGrade { get; set; }
    }
}