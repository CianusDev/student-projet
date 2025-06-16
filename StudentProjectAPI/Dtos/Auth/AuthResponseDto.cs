using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
        public string? Message { get; set; }
    }
}

