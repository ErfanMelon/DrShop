using Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.Account.Commands.RegisterUser
{
    public class RequestRegisterUser : IRequest<ResultDto<int>>
    {
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string Username { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }
        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }
        public BaseRole Role { get; set; }
    }
}
