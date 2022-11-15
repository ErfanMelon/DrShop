using Application.Services.Account.Commands.RegisterUser;
using Application.Services.Account.Queries.LoginUser;
using Dr_Shop.Models.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Controllers
{
    public class UserController:Controller
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
            var result = _loginUserService.Execute(new RequestUserLoginDto
            {
                Email = model.Email.Trim(),
                Password = model.Password
            });
            if (result.IsSuccess)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,result.Data.Username),
                    new Claim(ClaimTypes.Role,result.Data.Role)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                };
                await HttpContext.SignInAsync(claimsPrincipal, properties);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
