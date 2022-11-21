using Application.Interfaces;
using Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account.Queries.GetUserForEdit
{
    public interface IGetUserService
    {
        ResultDto<UserDto> Execute(int userId);
    }
    public class GetUserService : IGetUserService
    {
        private readonly IDataBaseContext _context;
        public GetUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<UserDto> Execute(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                UserDto resultUser = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = (BaseRole)user.RoleId,
                    Username = user.Username
                };
                return new ResultDto<UserDto> { Data = resultUser, IsSuccess = true };
            }
            return new ResultDto<UserDto> { Message = "کاربر پیدا نشد" };
        }
    }
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BaseRole Role { get; set; }
    }
    public class UserDtoValidation:AbstractValidator<UserDto>
    {
        public UserDtoValidation()
        {
            RuleFor(e => e.UserId).NotEmpty().WithMessage("کاربری یافت نشد");
            RuleFor(e => e.Username).NotEmpty().WithMessage("نام کاربری را وارد کنید");
            RuleFor(e => e.Email).EmailAddress().WithMessage("ایمیل معتبر نیست");
            RuleFor(e => e.Email).NotEmpty().WithMessage("ایمیل را وارد کنید");
            RuleFor(e => e.Role).IsInEnum().WithMessage("سطح دسترسی معتبر نیست");
        }
    }
}
