using StudentProjectAPI.Dtos.User;
using StudentProjectAPI.Services;

namespace StudentProjectAPI.Controllers
{
    /// Contrôleur gérant l'authentification des utilisateurs
    /// Constructeur du contrôleur d'authentification
    /// Service d'authentification injecté
    public class AuthController(IAuthService authService)
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Endpoint pour l'inscription d'un nouvel utilisateur
        /// </summary>
        /// <param name="registerDto">Données d'inscription de l'utilisateur</param>
        /// <returns>Réponse contenant le token JWT et les informations de l'utilisateur</returns>
        public async Task<AuthResponseDto> Register(RegisterUserDto registerDto)
        {
            return await _authService.RegisterAsync(registerDto);
        }
        
        /// <param name="loginDto">Données de connexion de l'utilisateur</param>
        /// <returns>Réponse contenant le token JWT et les informations de l'utilisateur</returns>
        public async Task<AuthResponseDto> Login(LoginUserDto loginDto)
        {
            return await _authService.LoginAsync(loginDto);
        }

        /// <summary>
        /// Endpoint pour le changement de mot de passe d'un utilisateur authentifié
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="changePasswordDto">Données de changement de mot de passe</param>
        /// <returns>Message de confirmation du changement de mot de passe</returns>
        public async Task<AuthResponseDto> ChangePassword(string userId, ChangePasswordDto changePasswordDto)
        {
            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
            return new AuthResponseDto { Message = "Mot de passe modifié avec succès" };
        }
    }
}