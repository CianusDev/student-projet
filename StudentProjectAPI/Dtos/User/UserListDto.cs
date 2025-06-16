using StudentProjectAPI.Dtos.User;

namespace StudentProjectAPI.Dtos.User
{
    public class UserListDto
    {
        public List<UserDto> Users { get; set; } = new();
    }
}
