using StudentProjectAPI.Models;

namespace StudentProjectAPI.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        // Ajoutez d'autres méthodes au besoin
    }
}
