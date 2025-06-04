using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTOs.Evaluation
{

    // DTO pour statistiques d'évaluation
    public class EvaluationStatsDto
    {
        public int TotalEvaluations { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public int PassingGrades { get; set; }
        public int FailingGrades { get; set; }
        public double PassRate => TotalEvaluations > 0 ? (double)PassingGrades / TotalEvaluations * 100 : 0;
    }
}
