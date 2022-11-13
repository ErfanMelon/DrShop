using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using DrShop.Models.AccountViewModel;
using Application.Services.Account.Commands.RegisterUser;
using Common;
using Application.Services.Account.Queries.LoginUser;
using DrShop.Utilities;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DrShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly ILoginUserService _loginUserService;
        public AccountController(IRegisterUserService registerUserService, ILoginUserService loginUserService)
        {
            _registerUserService = registerUserService;
            _loginUserService = loginUserService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [Route("/Login")]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var result = _loginUserService.Execute(new RequestUserLoginDto
            {
                Email = model.Email.Trim(),
                Password = model.Password
            });
            if (result.IsSuccess)
                LoginUser(result.Data.Username, result.Data.Role, result.Data.UserId);
            return Json(result);
        }
        [HttpPost]
        public IActionResult Signin(SigninViewModel model)
        {
            var result = _registerUserService.Execute(new RegisterUserDto
            {
                Email = model.Email.Trim(),
                Password = model.Password,
                Username = model.Username.Trim(),
                RoleId = (int)BaseRole.Customer
            });
            if (result.IsSuccess)
                LoginUser(model.Username.Trim(), Enum.GetName(BaseRole.Customer), result.Data);
            return Json(result);
        }
        public void LoginUser(string Name, string Role, int UserId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,UserId.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, "Customer"),
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = true
            };
            HttpContext.SignInAsync(principal, properties);
        }
        //public async void LoginUser(string Name, string Role, int UserId)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier,UserId.ToString()),
        //        new Claim(ClaimTypes.Name,Name),
        //        new Claim(ClaimTypes.Role,Role)
        //    };
        //    var claimsIdentity = new ClaimsIdentity(
        //    claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        //AllowRefresh = <bool>,
        //        // Refreshing the authentication session should be allowed.

        //        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //        // The time at which the authentication ticket expires. A 
        //        // value set here overrides the ExpireTimeSpan option of 
        //        // CookieAuthenticationOptions set with AddCookie.

        //        //IsPersistent = true,
        //        // Whether the authentication session is persisted across 
        //        // multiple requests. When used with cookies, controls
        //        // whether the cookie's lifetime is absolute (matching the
        //        // lifetime of the authentication ticket) or session-based.

        //        //IssuedUtc = <DateTimeOffset>,
        //        // The time at which the authentication ticket was issued.

        //        //RedirectUri = <string>
        //        // The full path or absolute URI to be used as an http 
        //        // redirect response value.
        //    };

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(claimsIdentity),
        //        authProperties);
        //    //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        //    //context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        //}
    }
}
