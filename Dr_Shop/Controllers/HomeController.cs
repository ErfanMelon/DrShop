using Application.Services.Product.Queries.GetProductForSite;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(int productnumbers=6)
        {
            var result = await _mediator.Send(new RequestGetProductSite(productnumbers));
            return View(result);
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