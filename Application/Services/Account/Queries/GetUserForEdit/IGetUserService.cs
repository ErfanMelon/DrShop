using Application.Interfaces;
using Common;
using FluentValidation;
using MediatR;

namespace Application.Services.Account.Queries.GetUserForEdit
{
    public record GetUserDto(int UserId) : IRequest<ResultDto<UserDto>>;
    public class GetUserService : IRequestHandler<GetUserDto, ResultDto<UserDto>>
    {
        private readonly IDataBaseContext _context;
        public GetUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<UserDto>> Handle(GetUserDto request, CancellationToken cancellationToken)
        {
            var user =_context.Users.Find(request.UserId);
            if (user != null)
            {
                UserDto resultUser = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = (BaseRole)user.RoleId,
                    Username = user.Username
                };
                return Task.FromResult(new ResultDto<UserDto> { Data = resultUser, IsSuccess = true });
            }
            return Task.FromResult(new ResultDto<UserDto> { Message = "کاربر پیدا نشد" });
        }
    }
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BaseRole Role { get; set; }
    }
    public class UserDtoValidation : AbstractValidator<UserDto>
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
