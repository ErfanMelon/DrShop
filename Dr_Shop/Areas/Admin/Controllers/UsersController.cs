using Application.Services.Account.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController:Controller
    {
        private readonly IGetUsersService _getUsersService;
        public UsersController(IGetUsersService getUsersService)
        {
            _getUsersService = getUsersService;
        }

        public IActionResult Index(int page=1,int pagesize=20)
        {
            var result = _getUsersService.Execute(page, pagesize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pagesize;

            return View(result.Data);
        }
    }
}
