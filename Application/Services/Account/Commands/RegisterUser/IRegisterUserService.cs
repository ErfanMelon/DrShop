using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using FluentValidation;
using MediatR;

namespace Application.Services.Account.Commands.RegisterUser
{
    public class RegisterUserDto : IRequest<ResultDto<int>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public BaseRole Role { get; set; }
    }
    public class RegisterUserService : IRequestHandler<RegisterUserDto, ResultDto<int>>
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<int>> Handle(RegisterUserDto request, CancellationToken cancellationToken)
        {
            if (_context.Users.Any(u => u.Email == request.Email) == true)
            {
                return new ResultDto<int> { Message = "ایمیل استفاده شده" };
            }

            User newUser = new User();
            newUser.Username = request.Username;
            newUser.Email = request.Email;
            PasswordHasher hasher = new PasswordHasher();
            newUser.RoleId = (int)request.Role;
            newUser.Password = hasher.HashPassword(request.Password);
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResultDto<int> { Data = newUser.UserId, IsSuccess = true, Message = "ثبت نام شدید" };
        }

        public class RegisterUserValidation : AbstractValidator<RegisterUserDto>
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
