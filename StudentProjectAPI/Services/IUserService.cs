using System;
using System.Threading.Tasks;

namespace StudentProjectAPI.Services
{
    /// <summary>
    /// Interface pour les opérations liées aux utilisateurs.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Supprime un utilisateur par son ID.
        /// </summary>
        /// <param name="userId">ID de l'utilisateur à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon false.</returns>
        Task<bool> DeleteUserAsync(int userId);
    }
}


