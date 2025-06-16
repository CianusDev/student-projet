using System.Threading.Tasks;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Services
{
    public interface IUserService
    {
        Task<bool> DeleteUserAsync(int userId);
        Task<UserDto?> GetUserByIdAsync(int userId);
    }
}



