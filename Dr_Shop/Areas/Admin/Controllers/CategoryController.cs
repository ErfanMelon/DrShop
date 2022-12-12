using Application.Services.Product.Commands.AddCategory;
using Application.Services.Product.Commands.DeleteCategory;
using Application.Services.Product.Commands.EditCategory;
using Application.Services.Product.Queries.GetCategories;
using Application.Services.Product.Queries.GetCategory;
using Dr_Shop.Models.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dr_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(string? Searchkey, int page = 1, int pagesize = 30)
        {
            var result = await _mediator.Send(new RequestGetCategories(page, pagesize, Searchkey));
            return View(result.Data);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.ParentCategories = await _mediator.Send(new RequestGetParentCategories());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RequestAddCategory model)
        {
            RequestAddCategoryValidation validationRules = new RequestAddCategoryValidation();
            var validModel = validationRules.Validate(model);
            if (validModel.IsValid)
            {
                var result = await _mediator.Send(new RequestAddCategory(
                                categoryName: model.categoryName.Trim(),
                                parentCategoryId: model.parentCategoryId));
                if (result.IsSuccess)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    ViewBag.Error = result.Message;
                }
            }
            else
            {
                ViewBag.Error = validModel.Errors[0].ErrorMessage;
            }
            ViewBag.ParentCategories = await _mediator.Send(new RequestGetParentCategories());
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new RequestGetCategory(id));
            ViewBag.ParentCategories = await _mediator.Send(new RequestGetParentCategories());
            return View(new RequestEditCategory
            {
                CategoryId = result.Data.CategoryId,
                CategoryName = result.Data.CategoryName,
                ParentCategory = result.Data.ParentCategoryId
            });

        }
        [HttpPost]
        public async Task<IActionResult> Edit(RequestEditCategory model)
        {
            RequestEditCategoryValidation validationRules = new RequestEditCategoryValidation();
            var validModel = validationRules.Validate(model);
            if (validModel.IsValid)
            {
                var result = await _mediator.Send(new RequestEditCategory
                {
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName.Trim(),
                    ParentCategory = model.ParentCategory
                });
                TempData["Success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = validModel.Errors[0].ErrorMessage;
            }
            ViewBag.ParentCategories = await _mediator.Send(new RequestGetParentCategories());
            return View(model);
        }
        [HttpPost]
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new RequestDeleteCategory(id));
            return Json(result);
        }
        [TypeFilter(typeof(JsonExceptionFilter))]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _mediator.Send(new RequestGetCategory(id));
            return PartialView(result.Data);
            return null;
        }
    }
}
