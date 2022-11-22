namespace Application.Services.Account.Queries.GetUsers
{
    /// <summary>
    /// Used for list of users
    /// </summary>
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
