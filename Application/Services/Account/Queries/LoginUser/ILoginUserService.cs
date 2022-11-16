using Application.Interfaces;
using Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account.Queries.LoginUser
{
    public interface ILoginUserService
    {
        ResultDto<UserLoginDetailDto> Execute(RequestUserLoginDto request);
    }
    public class LoginUserService : ILoginUserService
    {
        private readonly IDataBaseContext _context;
        public LoginUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<UserLoginDetailDto> Execute(RequestUserLoginDto request)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            var user = _context.Users.FirstOrDefault(e => e.Email == request.Email);
            if (user != null)
            {
                bool truePassword = passwordHasher.VerifyPassword(user.Password, request.Password);
                if (truePassword)
                {
                    return new ResultDto<UserLoginDetailDto>
                    {
                        Data = new UserLoginDetailDto
                        {
                            Username = user.Username,
                            UserId = user.UserId,
                            Role = Enum.GetName(typeof(BaseRole), user.RoleId)
                        },
                        IsSuccess = true,
                        Message = "خوش آمدید"
                    };
                }
                return new ResultDto<UserLoginDetailDto> { Message = "رمز عبور اشتباه است" };
            }
            return new ResultDto<UserLoginDetailDto> { Message = "کاربری یافت نشد" };

        }
    }
    public class RequestUserLoginDto
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
