using FluentValidation;

namespace Application.Services.Account.Queries.GetUserForEdit
{
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
