using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.Group;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des groupes de projet.
    /// </summary>
    public class GroupController(IGroupService groupService) : ControllerBase
    {
        private readonly IGroupService _groupService = groupService;

        /// <summary>
        /// Récupère la liste de tous les groupes.
        /// </summary>
        /// <returns>Liste des groupes</returns>
        [HttpGet]
        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            return await _groupService.GetAllGroupsAsync();
        }

        /// <summary>
        /// Récupère un groupe par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du groupe</param>
        /// <returns>Le groupe correspondant</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound();

            return group;
        }

        /// <summary>
        /// Crée un nouveau groupe.
        /// </summary>
        /// <param name="createDto">Données de création du groupe</param>
        /// <param name="studentId">Identifiant de l'étudiant créateur</param>
        /// <returns>Le groupe créé</returns>
        [HttpPost]
        public async Task<ActionResult<GroupDto>> CreateGroup([FromBody] CreateGroupDto createDto, [FromQuery] string studentId)
        {
            var group = await _groupService.CreateGroupAsync(createDto, studentId);
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        /// <summary>
        /// Met à jour un groupe existant.
        /// </summary>
        /// <param name="id">Identifiant du groupe</param>
        /// <param name="updateDto">Données de mise à jour</param>
        /// <param name="studentId">Identifiant de l'étudiant effectuant la modification</param>
        /// <returns>Le groupe mis à jour</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupDto>> UpdateGroup(int id, [FromBody] UpdateGroupDto updateDto, [FromQuery] string studentId)
        {
            var group = await _groupService.UpdateGroupAsync(id, updateDto, studentId);
            if (group == null)
                return NotFound();

            return group;
        }

        /// <summary>
        /// Supprime un groupe.
        /// </summary>
        /// <param name="id">Identifiant du groupe</param>
        /// <param name="studentId">Identifiant de l'étudiant effectuant la suppression</param>
        /// <returns>Résultat de la suppression</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id, [FromQuery] string studentId)
        {
            var result = await _groupService.DeleteGroupAsync(id, studentId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Ajoute un membre à un groupe.
        /// </summary>
        /// <param name="id">Identifiant du groupe</param>
        /// <param name="studentId">Identifiant de l'étudiant effectuant l'ajout</param>
        /// <param name="newMemberId">Identifiant du nouvel étudiant à ajouter</param>
        /// <returns>Résultat de l'ajout</returns>
        [HttpPost("{id}/members")]
        public async Task<ActionResult> AddMember(int id, [FromQuery] string studentId, [FromQuery] string newMemberId)
        {
            var result = await _groupService.AddMemberAsync(id, studentId, newMemberId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Supprime un membre d'un groupe.
        /// </summary>
        /// <param name="id">Identifiant du groupe</param>
        /// <param name="memberId">Identifiant du membre à supprimer</param>
        /// <param name="studentId">Identifiant de l'étudiant effectuant la suppression</param>
        /// <returns>Résultat de la suppression</returns>
        [HttpDelete("{id}/members/{memberId}")]
        public async Task<ActionResult> RemoveMember(int id, string memberId, [FromQuery] string studentId)
        {
            var result = await _groupService.RemoveMemberAsync(id, studentId, memberId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Récupère les groupes auxquels appartient un étudiant.
        /// </summary>
        /// <param name="studentId">Identifiant de l'étudiant</param>
        /// <returns>Liste des groupes de l'étudiant</returns>
        [HttpGet("student/{studentId}")]
        public async Task<IEnumerable<StudentGroupDto>> GetStudentGroups(string studentId)
        {
            return await _groupService.GetStudentGroupsAsync(studentId);
        }
    }
} 