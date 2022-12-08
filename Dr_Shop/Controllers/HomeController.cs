using Dr_Shop.Models.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    return View("Error403");
                case 400:
                    ViewBag.ErrorCode = statusCode;
                    return View("Error404");
                default:
                    ViewBag.ErrorCode = 404;
                    return View("Error404");
            }
        }
    }
}