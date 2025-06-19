using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Services;
using StudentProjectAPI.Dtos.Submission;
using StudentProjectAPI.DTOs.Submission;

namespace StudentProjectAPI.Controllers
{
    public class SubmissionController(ISubmissionService submissionService) : ControllerBase
    {
        private readonly ISubmissionService _submissionService = submissionService;

        // / <summary>
        // / Crée une nouvelle soumission de livrable (avec fichier).
        // / </summary>
        // / <param name="dto">Données de la soumission (formulaire + fichier)</param>
        // / <returns>La soumission créée</returns>
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateSubmission([FromForm] CreateSubmissionWithFileDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type.EndsWith("nameidentifier"))?.Value;
            if (userId == null)
                return Unauthorized();
            var result = await _submissionService.CreateSubmissionAsync(
                new CreateSubmissionDto
                {
                    AssignmentId = dto.AssignmentId,
                    DeliverableId = dto.DeliverableId,
                    Comments = dto.Comments
                },
                userId,
                dto.File
            );
            return Ok(result);
        }

        /// <summary>
        /// Récupère une soumission par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la soumission</param>
        /// <returns>La soumission correspondante</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            var result = await _submissionService.GetSubmissionByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Liste toutes les soumissions pour une assignation donnée.
        /// </summary>
        /// <param name="assignmentId">Identifiant de l'assignation</param>
        /// <returns>Liste des soumissions</returns>
        [HttpGet("assignment/{assignmentId}")]
        [Authorize]
        public async Task<IActionResult> GetSubmissionsByAssignment(int assignmentId)
        {
            var result = await _submissionService.GetSubmissionsByAssignmentAsync(assignmentId);
            return Ok(result);
        }

        /// <summary>
        /// Télécharge le fichier d'une soumission.
        /// </summary>
        /// <param name="submissionId">Identifiant de la soumission</param>
        /// <returns>Le fichier à télécharger</returns>
        [HttpGet("download/{submissionId}")]
        [Authorize]
        public async Task<IActionResult> DownloadSubmissionFile(int submissionId)
        {
            var fileDto = await _submissionService.DownloadSubmissionFileAsync(submissionId);
            if (fileDto == null) return NotFound();
            return File(fileDto.FileContent, fileDto.ContentType, fileDto.FileName);
        }
    }
} 