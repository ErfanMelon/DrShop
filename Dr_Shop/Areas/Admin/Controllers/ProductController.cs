using Application.Services.Product.Commands.AddComment;
using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Commands.DeleteComment;
using Application.Services.Product.Commands.DeleteProduct;
using Application.Services.Product.Commands.EditProduct;
using Application.Services.Product.Queries.GetCategories;
using Application.Services.Product.Queries.GetProduct;
using Application.Services.Product.Queries.GetProducts;
using Application.Services.Product.Queries.SearchProducts;
using Dr_Shop.Models.Filters;
using Dr_Shop.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(int page = 1, int pagesize = 20)
        {
            var result = await _mediator.Send(new RequestGetProducts(page, pagesize));
            GetCategories();
            return View(result.Data);
        }
        public IActionResult Create()
        {
            GetCategories();
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _mediator.Send(new RequestGetProduct(id));
            return View(result.Data);
        }
        private void GetCategories()
        {
            var result = _mediator.Send(new RequestGetCategories(1));
            var parentcategory = result.Result.Data.Items
                .Where(e => e.ParentCategory == " - ")
                .Select(e => new SelectListGroup
                {
                    Name = e.CategoryName
                }).ToDictionary(e => e.Name);
            List<SelectListItem> categories = result.Result.Data.Items.Where(e => e.ParentCategory != " - ").Select(e => new SelectListItem
            {
                Text = e.CategoryName,
                Value = e.CategoryId.ToString(),
                Group = parentcategory[e.ParentCategory]
            }).ToList();
            ViewBag.Categories = categories;
        }

        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Create(RequestAddProduct model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new RequestDeleteProduct(id));
            return Json(result);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new RequestGetProductForEdit(id));
            GetCategories();
            return View(result.Data);
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Edit(RequestEditProduct model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }
        public async Task<IActionResult> Search(int page = 1, int pagesize = 20, int categoryid = 0, SortBy order = SortBy.News, string searchkey = "")
        {
            var result = await _mediator.Send(new RequestSearchProduct
            {
                Page = page,
                PageSize = pagesize,
                CategoryId = categoryid,
                Order = order,
                SearchKey = searchkey
            });
            GetCategories();
            return View(result.Data);
        }
        public IActionResult AddComment()
        {
            return PartialView();
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> AddComment(RequestAddComment model)
        {
            model.UserId = (int)ClaimUtility.GetUserId(User).Value;
            var result = await _mediator.Send(model);
            return Json(result);
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _mediator.Send(new RequestDeleteComment(id));
            return Json(result);
        }
    }
}
