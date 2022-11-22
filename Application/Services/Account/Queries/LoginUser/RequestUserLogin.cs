using Common;
using MediatR;

namespace Application.Services.Account.Queries.LoginUser
{
    public class RequestUserLogin : IRequest<ResultDto<UserLoginDetailDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
