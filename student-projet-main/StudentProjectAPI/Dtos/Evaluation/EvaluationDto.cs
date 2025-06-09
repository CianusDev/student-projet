using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Evaluation
{

    // DTO pour afficher une évaluation
    public class EvaluationDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int TeacherId { get; set; }
        public UserDto Teacher { get; set; } = null!;
        public decimal? OverallGrade { get; set; }
        public string? OverallComments { get; set; }
        public DateTime EvaluatedAt { get; set; }
        public List<DeliverableEvaluationDto> DeliverableEvaluations { get; set; } = new();
    }
}