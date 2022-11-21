using Application.Interfaces;
using Common;
using MediatR;

namespace Application.Services.Account.Queries.LoginUser
{
    public class LoginUserService : IRequestHandler<RequestUserLoginDto, ResultDto<UserLoginDetailDto>>
    {
        private readonly IDataBaseContext _context;
        public LoginUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<UserLoginDetailDto>> Handle(RequestUserLoginDto request, CancellationToken cancellationToken)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            var user = _context.Users.FirstOrDefault(e => e.Email == request.Email);
            if (user != null)
            {
                bool truePassword = passwordHasher.VerifyPassword(user.Password, request.Password);
                if (truePassword)
                {
                    return Task.FromResult(new ResultDto<UserLoginDetailDto>
                    {
                        Data = new UserLoginDetailDto
                        {
                            Username = user.Username,
                            UserId = user.UserId,
                            Role = Enum.GetName(typeof(BaseRole), user.RoleId)
                        },
                        IsSuccess = true,
                        Message = "خوش آمدید"
                    });
                }
                return Task.FromResult(new ResultDto<UserLoginDetailDto> { Message = "رمز عبور اشتباه است" });
            }
            return Task.FromResult(new ResultDto<UserLoginDetailDto> { Message = "کاربری یافت نشد" });
        }
    }
    public class RequestUserLoginDto : IRequest<ResultDto<UserLoginDetailDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginDetailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
