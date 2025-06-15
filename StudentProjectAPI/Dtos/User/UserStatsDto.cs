using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Dtos.User
{
    public class UserStatsDto
    {
        public int TotalProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int PendingProjects { get; set; }
        public int TotalGroups { get; set; }
        public double AverageGrade { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int Teachers { get; set; }
        public int Students { get; set; }
    }
}