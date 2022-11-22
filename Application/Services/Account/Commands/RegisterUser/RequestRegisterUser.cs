using Common;
using MediatR;

namespace Application.Services.Account.Commands.RegisterUser
{
    public class RequestRegisterUser : IRequest<ResultDto<int>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BaseRole Role { get; set; }
    }
}
