using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Account
{
    public class User
    {
        public int UserId { get; set; }
        [StringLength(50,MinimumLength =3)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
