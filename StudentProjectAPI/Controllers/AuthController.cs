using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Routes;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(AuthRoutes.Auth.Register)]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto registerDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost(AuthRoutes.Auth.Login)]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginUserDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost(AuthRoutes.Auth.ChangePassword)]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
                return Ok(new { message = "Mot de passe modifié avec succès" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ✅ Endpoint pour supprimer un utilisateur (accessible uniquement aux administrateurs ou enseignants, à adapter)
        [Authorize(Roles = "Admin,Teacher")] // ← Tu peux adapter les rôles selon ton besoin
        [HttpDelete(AuthRoutes.Auth.DeleteUser + "/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _authService.DeleteUserAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Utilisateur non trouvé." });
                }

                return NoContent(); // 204, pas besoin de message dans le body
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

