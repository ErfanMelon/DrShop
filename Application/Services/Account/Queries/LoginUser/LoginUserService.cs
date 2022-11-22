using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Account.Queries.LoginUser
{
    public class LoginUserService : IRequestHandler<RequestUserLogin, ResultDto<UserLoginDetailDto>>
    {
        private readonly IDataBaseContext _context;
        public LoginUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<UserLoginDetailDto>> Handle(RequestUserLogin request, CancellationToken cancellationToken)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(e => e.Email == request.Email);
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
}
