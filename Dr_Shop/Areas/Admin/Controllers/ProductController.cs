﻿using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Commands.DeleteProduct;
using Application.Services.Product.Commands.EditProduct;
using Application.Services.Product.Queries.GetCategories;
using Application.Services.Product.Queries.GetProduct;
using Application.Services.Product.Queries.GetProducts;
using MediatR;
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
            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            TempData["Error"] = result.Message;
            return BadRequest();
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
        public async Task<IActionResult> Create(RequestAddProduct model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new RequestDeleteProduct(id));
            return Json(result);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new RequestGetProductForEdit(id));
            if (result.IsSuccess)
            {
                GetCategories();
                return View(result.Data);
            }
            ViewBag.Error = result.Message;
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RequestEditProduct model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }
    }
}