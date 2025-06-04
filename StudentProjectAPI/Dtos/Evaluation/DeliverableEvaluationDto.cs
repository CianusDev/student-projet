using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.Evaluation
{

    // DTO pour évaluation d'un livrable
    public class DeliverableEvaluationDto
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string DeliverableTitle { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        public string? Comments { get; set; }
    }
}