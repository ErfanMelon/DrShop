using Application.Services.Account.Commands.DeleteUser;
using Application.Services.Account.Commands.EditUser;
using Application.Services.Account.Commands.RegisterUser;
using Application.Services.Account.Queries.GetUserForEdit;
using Application.Services.Account.Queries.GetUsers;
using Common;
using Dr_Shop.Models.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using static Application.Services.Account.Commands.RegisterUser.RegisterUserService;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(string? Searchkey, int page = 1, int pagesize = 20)
        {
            var result = await _mediator.Send(new RequestGetUsers(page, pagesize, Searchkey));
            return View(result.Data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new RequestGetUser(id));
            GetRolesForSelectList();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            ResultDto result;
            UserDtoValidation validationRules = new UserDtoValidation();
            var validModel = validationRules.Validate(model);
            if (validModel.IsValid)
            {
                result = await _mediator.Send(new RequestEditUser
                {
                    Email = model.Email.Trim(),
                    Role = model.Role,
                    UserId = model.UserId,
                    Username = model.Username.Trim(),
                });
                TempData["Success"] = result.Message;
                return RedirectToAction("Index", "Users");
            }
            else
            {
                result = new ResultDto { Message = validModel.Errors[0].ErrorMessage };
            }
            GetRolesForSelectList();
            ViewBag.Error = result.Message;
            return View(model);
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new RequestDeleteUser(id));
            return Json(result);
        }
        public IActionResult Create()
        {
            GetRolesForSelectList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RequestRegisterUser model)
        {
            if (ModelState.IsValid)
            {
                ResultDto<int> result;
                RegisterUserValidation validationRules = new RegisterUserValidation();
                var validation = validationRules.Validate(model);
                if (validation.IsValid)
                {
                    result = await _mediator.Send(new RequestRegisterUser
                    {
                        Email = model.Email.Trim(),
                        Role = model.Role,
                        Username = model.Username.Trim(),
                        Password = model.Password,
                    });
                    if (result.IsSuccess)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Index", "Users");
                    }
                }
                else
                {
                    result = new ResultDto<int> { Message = validation.Errors[0].ErrorMessage };
                }
                GetRolesForSelectList();
                ViewBag.Error = result.Message;
                return View(model);
            }
            return View(model);
        }
        /// <summary>
        /// Get Roles From Enum
        /// </summary>
        private void GetRolesForSelectList()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(BaseRole)).Cast<BaseRole>().Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = ((int)e).ToString()
            }), "Value", "Text");
        }
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _mediator.Send(new RequestGetUser(id));
            return View(result.Data);
        }
    }
}
