using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using DrShop.Models.AccountViewModel;
using Application.Services.Account.Commands.RegisterUser;

namespace DrShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegisterUserService RegisterUserService;
        public AccountController(IRegisterUserService registerUserService)
        {
            RegisterUserService = registerUserService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
        [Route("/Login")]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return Json("");
        }
        [HttpPost]
        public IActionResult Signin(SigninViewModel model)
        {
            var result = RegisterUserService.Execute(new RegisterUserDto
            {
                Email=model.Email,
                Password=model.Password,
                Username=model.Username,
                RoleId=2
            });
            return Json(result);
        }
    }
}
