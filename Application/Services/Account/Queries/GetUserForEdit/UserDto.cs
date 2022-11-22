using Common;

namespace Application.Services.Account.Queries.GetUserForEdit
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BaseRole Role { get; set; }
    }
}
