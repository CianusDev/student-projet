using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.Auth;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    /// <summary>
    /// Contrôleur gérant l'authentification des utilisateurs
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inscription d’un nouvel utilisateur
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Connexion d’un utilisateur
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Changement de mot de passe pour un utilisateur
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromQuery] int userId, [FromBody] ChangePasswordDto dto)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(userId, dto);
                if (!result)
                    return BadRequest(new { message = "Échec du changement de mot de passe." });

                return Ok(new { message = "Mot de passe modifié avec succès." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

