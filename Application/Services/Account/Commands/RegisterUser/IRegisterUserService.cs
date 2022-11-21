using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using FluentValidation;

namespace Application.Services.Account.Commands.RegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<int> Execute(RegisterUserDto request);
    }
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BaseRole Role { get; set; }
    }
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<int> Execute(RegisterUserDto request)
        {
            if (_context.Users.Any(u => u.Email == request.Email)==true)
            {
                return new ResultDto<int> { Message = "ایمیل استفاده شده" };
            }

            User newUser = new User();
            newUser.Username = request.Username;
            newUser.Email = request.Email;
            PasswordHasher hasher = new PasswordHasher();
            newUser.RoleId = (int)request.Role;
            newUser.Password = hasher.HashPassword(request.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return new ResultDto<int> { Data=newUser.UserId, IsSuccess = true, Message = "ثبت نام شدید" };
        }
        public class RegisterUserValidation:AbstractValidator<RegisterUserDto>
        {
            public RegisterUserValidation()
            {
                RuleFor(e => e.Email).EmailAddress().NotEmpty().WithMessage("ایمیل صحیح نیست");
                RuleFor(e => e.Username).NotEmpty().WithMessage("نام کاربری درست نیست");
                RuleFor(e => e.Password).NotEmpty().WithMessage("رمز را وارد کنید");
                RuleFor(e => e.Role).IsInEnum().WithMessage("نقش را وارد کنید");
            }
        }
    }
}
