using FluentValidation;

namespace Application.Services.Account.Commands.RegisterUser
{
    public partial class RegisterUserService
    {
        public class RegisterUserValidation : AbstractValidator<RequestRegisterUser>
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
