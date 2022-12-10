using Application.Services.Product.Queries.GetProductBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Controllers
{
    public class ProductController:Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("/Product/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _mediator.Send(new RequestGetProductBySlug(slug));
            return View(product.Data);
        }
    }
}
