using Application.Services.Account.Commands.DeleteUser;
using Application.Services.Account.Commands.EditUser;
using Application.Services.Account.Commands.RegisterUser;
using Application.Services.Account.Queries.GetUserForEdit;
using Application.Services.Account.Queries.GetUsers;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.Account.Commands.RegisterUser.RegisterUserService;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetUserService _getUserService;
        private readonly IEditUserService _editUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IRegisterUserService _registerUserService;
        public UsersController(IGetUsersService getUsersService, IGetUserService getUserService, IEditUserService editUserService, IDeleteUserService deleteUserService, IRegisterUserService registerUserService)
        {
            _getUsersService = getUsersService;
            _getUserService = getUserService;
            _editUserService = editUserService;
            _deleteUserService = deleteUserService;
            _registerUserService = registerUserService;
        }

        public IActionResult Index(int page = 1, int pagesize = 20)
        {
            var result = _getUsersService.Execute(page, pagesize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pagesize;

            return View(result.Data);
        }
        public IActionResult Edit(int id)
        {
            var result = _getUserService.Execute(id);
            if (result.IsSuccess)
            {
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(BaseRole)).Cast<BaseRole>().Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                }), "Value", "Text");
                return View(result.Data);
            }

            TempData["Error"] = result.Message;
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Edit(UserDto model)
        {
            ResultDto result;
            UserDtoValidation validationRules = new UserDtoValidation();
            var validModel = validationRules.Validate(model);
            if (validModel.IsValid)
            {
                result = _editUserService.Execute(new RequestEditUserDto
                {
                    Email = model.Email,
                    RoleId = model.RoleId,
                    UserId = model.UserId,
                    Username = model.Username
                });
                if (result.IsSuccess)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Index", "Users");
                }

            }
            else
            {
                result = new ResultDto { Message = validModel.Errors[0].ErrorMessage };
            }
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(BaseRole)).Cast<BaseRole>().Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = ((int)e).ToString()
            }), "Value", "Text");
            ViewBag.Error = result.Message;
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _deleteUserService.Execute(id);
            return Json(result);
        }
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(BaseRole)).Cast<BaseRole>().Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = ((int)e).ToString()
            }), "Value", "Text");
            return View();
        }
        [HttpPost]
        public IActionResult Create(RegisterUserDto model)
        {
            ResultDto<int> result;
            RegisterUserValidation validationRules = new RegisterUserValidation();
            var validation = validationRules.Validate(model);
            if (validation.IsValid)
            {
                result = _registerUserService.Execute(model);
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
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(BaseRole)).Cast<BaseRole>().Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = ((int)e).ToString()
            }), "Value", "Text");
            ViewBag.Error = result.Message;
            return View(model);
        }
    }
}
