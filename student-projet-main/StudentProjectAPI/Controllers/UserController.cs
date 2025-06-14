using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
    private static readonly List<RegisterUserDto> Users = new();

    
    
        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto updateDto)
        {
            // Recherche d'un utilisateur
            var user = Users.FirstOrDefault(u => u.Email == "email@example.com");

            if (user == null)
                return NotFound("Utilisateur non trouvé");

            // Mise à jour conditionnelle
            if (!string.IsNullOrEmpty(updateDto.FirstName)) user.FirstName = updateDto.FirstName;
            if (!string.IsNullOrEmpty(updateDto.LastName)) user.LastName = updateDto.LastName;
            if (!string.IsNullOrEmpty(updateDto.Email)) user.Email = updateDto.Email;

            return Ok(user); // ou NoContent()
        }
    }
}
