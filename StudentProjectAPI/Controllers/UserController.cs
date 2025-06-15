using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IEnumerable<UserListItemDto>> GetUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<UserDto?> GetUser(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<UserDto?> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            return await _userService.UpdateUserAsync(id, updateDto);
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        // GET: api/user/stats
        [HttpGet("stats")]
        public async Task<UserStatsDto> GetUserStats()
        {
            return await _userService.GetUserStatsAsync();
        }
    }
}
