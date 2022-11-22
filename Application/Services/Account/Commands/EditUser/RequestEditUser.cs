using Common;
using MediatR;

namespace Application.Services.Account.Commands.EditUser
{
    public class RequestEditUser : IRequest<ResultDto>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BaseRole Role { get; set; }
    }
}
