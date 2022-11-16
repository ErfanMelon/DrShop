using Application.Services.Account.Commands.RegisterUser;
using Application.Services.Account.Queries.LoginUser;
using Common;
using Dr_Shop.Models.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dr_Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly ILoginUserService _loginUserService;
        public UserController(IRegisterUserService registerUserService, ILoginUserService loginUserService)
        {
            _registerUserService = registerUserService;
            _loginUserService = loginUserService;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ResultDto<UserLoginDetailDto> result;
            var validationRules = new LoginViewModelValidation();
            var validation = validationRules.Validate(model); // validate LoginViewModel using FluentValidation
            if (validation.IsValid)
            {
                result = _loginUserService.Execute(new RequestUserLoginDto
                {
                    Email = model.Email.Trim(),
                    Password = model.Password
                });
                if (result.IsSuccess)
                {
                    await SetAuthentications(result.Data.Username, result.Data.UserId.ToString(), result.Data.Role);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                result = new ResultDto<UserLoginDetailDto> { Message = validation.Errors[0].ErrorMessage };
            }


            ViewBag.Error = result.Message;
            return View(model);
        }
        [HttpGet("/Signup")]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost("/Signup")]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            ResultDto<int> result;
            SignupViewModelValidation validationRules = new SignupViewModelValidation();
            var validation = validationRules.Validate(model); // validate SignupViewModel using FluentValidation
            if (validation.IsValid)
            {
                result = _registerUserService.Execute(new RegisterUserDto
                {
                    Email = model.Email.Trim(),
                    Password = model.Password,
                    RoleId = (int)BaseRole.Customer,
                    Username = model.Username.Trim(),
                });
                if (result.IsSuccess)
                {
                    await SetAuthentications(model.Username, result.Data.ToString(), Enum.GetName(BaseRole.Customer));
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                result = new ResultDto<int> { Message = validation.Errors[0].ErrorMessage };
            }
            ViewBag.Error = result.Message;
            return View(model);
        }
        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        async Task SetAuthentications(string username, string userid, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(ClaimTypes.NameIdentifier,userid),
                    new Claim(ClaimTypes.Role,role)
                };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };
            await HttpContext.SignInAsync(claimsPrincipal, properties);
        }
    }
}
