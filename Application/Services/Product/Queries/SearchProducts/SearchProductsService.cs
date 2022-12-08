using Application.Interfaces;
using Application.Services.Product.Queries.GetProducts;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Product.Queries.SearchProducts
{
    public class SearchProductsService : IRequestHandler<RequestSearchProduct, ResultDto<ResultSearchProduct>>
    {
        private readonly IDataBaseContext _context;

        public SearchProductsService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<ResultSearchProduct>> Handle(RequestSearchProduct request, CancellationToken cancellationToken)
        {
            var searchResult = _context.Products
                .Include(e => e.Category)
                .Include(e => e.ProductImages)
                .AsNoTracking()
                .AsQueryable();
            if (request.CategoryId != 0)
            {
                searchResult = searchResult.Where(e => e.Category.CategoryId == request.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                string searchkey = request.SearchKey.Trim();
                searchResult = searchResult.Where(e =>
                e.Name.Contains(searchkey) ||
                e.Description.Contains(searchkey) ||
                e.ShortDescription.Contains(searchkey)
                );
            }
            switch (request.Order)
            {
                case SortBy.News:
                    searchResult = searchResult.OrderBy(e => e.InsertTime);
                    break;
                case SortBy.Oldest:
                    searchResult = searchResult.OrderByDescending(e => e.InsertTime);
                    break;
                case SortBy.Cheapest:
                    searchResult = searchResult.OrderBy(e => e.Price);
                    break;
                case SortBy.Most_Expensive:
                    searchResult = searchResult.OrderByDescending(e => e.Price);
                    break;
                case SortBy.Visit:
                    searchResult = searchResult.OrderBy(e => e.Visits);
                    break;
                default:
                    break;
            }
            var products = searchResult
                .ToPaged(request.Page, request.PageSize, out int rowsCount)
                .Select(e => new ProductListDto
                {
                    Image = e.ProductImages.FirstOrDefault()?.Src ?? "",
                    Price = e.Price,
                    ProductId = e.ProductId,
                    ProductName = e.Name

                }).DefaultIfEmpty(new ProductListDto { Image = "", Price = 0, ProductId = 0, ProductName = "بدون محصول" });
            return Task.FromResult(new ResultDto<ResultSearchProduct>
            {
                IsSuccess = true,
                Data = new ResultSearchProduct
                {
                    Products = products.ToList(),
                    RequestSearch = request,
                    RowsCount = rowsCount,
                }
            });
        }
    }
    public class RequestSearchProduct : IRequest<ResultDto<ResultSearchProduct>>
    {
        public string SearchKey { get; set; }
        public SortBy Order { get; set; }
        public int CategoryId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public enum SortBy:int
    {
        [Display(Name = "جدیدترین")]
        News=10,
        [Display(Name = "قدیمیترین")]
        Oldest=20,
        [Display(Name = "ارزانترین")]
        Cheapest=40,
        [Display(Name = "گرانترین")]
        Most_Expensive=30,
        [Display(Name = "پربازدید")]
        Visit=70
    }
    public class ResultSearchProduct
    {
        public RequestSearchProduct RequestSearch { get; set; }
        public List<ProductListDto> Products { get; set; }
        public int RowsCount { get; set; }

    }
}
