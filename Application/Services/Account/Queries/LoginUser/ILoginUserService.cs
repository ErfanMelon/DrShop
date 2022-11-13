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
        private readonly UserLoginValidation _validationRules;
        public LoginUserService(IDataBaseContext context, UserLoginValidation validationRules)
        {
            _context = context;
            _validationRules = validationRules;
        }

        public ResultDto<UserLoginDetailDto> Execute(RequestUserLoginDto request)
        {
            var validation = _validationRules.Validate(request);
            if (!validation.IsValid)
            {
                return new ResultDto<UserLoginDetailDto> { Messege = validation.Errors[0].ErrorMessage };
            }
            PasswordHasher passwordHasher = new PasswordHasher();

            var user = _context.Users.First(e => e.Email == request.Email);
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
                    Messege = "خوش آمدید"
                };
            }
            return new ResultDto<UserLoginDetailDto> { Messege = "رمز عبور اشتباه است" };
        }
    }
    public class UserLoginValidation : AbstractValidator<RequestUserLoginDto>
    {
        private readonly IDataBaseContext _context;
        public UserLoginValidation(IDataBaseContext context)
        {
            _context = context;

            RuleFor(e => e.Email).NotEmpty().WithMessage("ایمیل را وارد کنید");
            RuleFor(e => e.Email).EmailAddress().WithMessage("ایمیل معتبر نمیباشد");
            RuleFor(e => e.Password).NotEmpty().WithMessage("رمز عبور را وارد کنید");
            RuleFor(e => e.Email).Must(e => _context.Users.Any(u => u.Email == e)).WithMessage("این ایمیل ثبت نام نکرده است");

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
