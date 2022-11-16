using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Models.UserViewModels
{
    public class SignupViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
    public class SignupViewModelValidation:AbstractValidator<SignupViewModel>
    {
        public SignupViewModelValidation()
        {
            RuleFor(e => e.Email).EmailAddress().NotEmpty().WithMessage("ایمیل صحیح نیست");
            RuleFor(e => e.Username).NotEmpty().WithMessage("نام کاربری درست نیست");
            RuleFor(e => e.Password).NotEmpty().WithMessage("رمز را وارد کنید");
            RuleFor(e => e.RePassword).NotEmpty().WithMessage("تکرار رمز را وارد کنید");
            RuleFor(e => e.RePassword).Equal(e=>e.Password).WithMessage("رمز و تکرار آن برابر نیست");
        }
    }
}
