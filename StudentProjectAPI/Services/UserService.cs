using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Data;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserListItemDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto?> UpdateUserAsync(string id, UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(string id);
        Task<UserStatsDto> GetUserStatsAsync();
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserListItemDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.GroupMemberships)
                .Include(u => u.IndividualAssignments)
                .Include(u => u.TeacherProjects)
                .ToListAsync();

            var userDtos = new List<UserListItemDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Student";

                userDtos.Add(new UserListItemDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Role = role
                });
            }

            return userDtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.GroupMemberships)
                .Include(u => u.IndividualAssignments)
                .Include(u => u.TeacherProjects)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Student";

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto?> UpdateUserAsync(string id, UpdateUserDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;

            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Student";

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserStatsDto> GetUserStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.EmailConfirmed);
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var students = await _userManager.GetUsersInRoleAsync("Student");

            return new UserStatsDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                Teachers = teachers.Count,
                Students = students.Count
            };
        }
    }
} 