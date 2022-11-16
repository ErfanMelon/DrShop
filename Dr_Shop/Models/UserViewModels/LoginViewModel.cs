using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Models.UserViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginViewModelValidation:AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidation()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("ایمیل را وارد کنید");
            RuleFor(e => e.Email).EmailAddress().WithMessage("ایمیل معتبر نمیباشد");
            RuleFor(e => e.Password).NotEmpty().WithMessage("رمز عبور را وارد کنید");
        }
    }
}
