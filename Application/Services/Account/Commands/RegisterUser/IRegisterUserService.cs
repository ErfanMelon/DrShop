using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account.Commands.RegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto Execute(RegisterUserDto request);
    }
    public class RegisterUserValidation : AbstractValidator<RegisterUserDto>
    {
        private readonly IDataBaseContext _context;
        public RegisterUserValidation(IDataBaseContext context)
        {
            _context = context;
            RuleFor(e => e.Email).EmailAddress().NotEmpty().WithMessage("ایمیل صحیح نیست");
            RuleFor(e => e.RoleId).NotEqual(0).WithMessage("سطح دسترسی صحیح نمیباشد");
            RuleFor(e => e.Username).NotEmpty().WithMessage("نام کاربری درست نیست");
            RuleFor(e => e.Password).NotEmpty().WithMessage("رمز صحیح نیست");
            RuleFor(e => e.Email).Must(uniqueEmail).WithMessage("ایمیل استفاده شده");
        }

        private bool uniqueEmail(string arg)
        {
            return !_context.Users.Any(u => u.Email == arg);
        }
    }
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        private RegisterUserValidation _validationRules;
        public RegisterUserService(IDataBaseContext context, RegisterUserValidation validationRules)
        {
            _context = context;
            _validationRules = validationRules;
        }

        public ResultDto Execute(RegisterUserDto request)
        {
            var validation = _validationRules.Validate(request);
            if (!validation.IsValid)
            {
                return new ResultDto { Messege = validation.Errors[0].ErrorMessage };
            }

            User newUser = new User();
            newUser.Username = request.Username;
            newUser.Email = request.Email;
            PasswordHasher hasher = new PasswordHasher();
            newUser.RoleId = request.RoleId;
            newUser.Password = hasher.HashPassword(request.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return new ResultDto { IsSuccess = true, Messege = "ثبت نام شدید" };
        }
    }
}
