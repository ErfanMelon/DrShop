using Microsoft.AspNetCore.Mvc;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
