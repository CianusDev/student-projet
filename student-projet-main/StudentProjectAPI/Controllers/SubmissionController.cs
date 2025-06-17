using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.Submission;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionController : ControllerBase
    {
        // Simulation de stockage temporaire
        private static readonly List<SubmissionDto> Submissions = new();
      
        [HttpPost]
        public IActionResult SubmitProject([FromBody] SubmissionDto submission)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Génération ID auto (à remplacer par DB plus tard)
            submission.Id = Submissions.Count + 1;
            submission.SubmittedAt = DateTime.UtcNow;
            submission.IsLate = submission.SubmittedAt > DateTime.UtcNow.AddDays(-1); 

            Submissions.Add(submission);

            return Ok(new
            {
                message = "Soumission enregistrée avec succès",
                data = submission
            });
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Submissions);
        }

        // Méthode pour formatter la taille du fichier
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
