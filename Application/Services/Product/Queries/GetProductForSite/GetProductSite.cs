using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProductForSite
{
    public class GetProductSite : IRequestHandler<RequestGetProductSite, PaginationDto<ProductSiteDto>>
    {
        private readonly IDataBaseContext context;

        public GetProductSite(IDataBaseContext context)
        {
            this.context = context;
        }

        public Task<PaginationDto<ProductSiteDto>> Handle(RequestGetProductSite request, CancellationToken cancellationToken)
        {
            var products = context.Products
                .Include(e => e.ProductImages)
                .ToPaged(1, request.count, out _)
                .Select(e => new ProductSiteDto
                {
                    Price = e.Price,
                    ProductImg = e.ProductImages.FirstOrDefault()?.Src ?? "",
                    ProductName = e.Name,
                    Slug = e.Slug
                })
                .ToList()
                .DefaultIfEmpty(new ProductSiteDto { Price = 0, Slug = "none", ProductImg = "", ProductName = "بدون محصول" });
            return Task.FromResult(new PaginationDto<ProductSiteDto>
            {
                Items = products.ToList(),
                PageNumber = 1,
                PageSize = request.count,
                TotalCount = 1
            });
        }
    }
    public record RequestGetProductSite(int count) : IRequest<PaginationDto<ProductSiteDto>>;
    public class ProductSiteDto
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Slug { get; set; }
        public string ProductImg { get; set; }
    }
}
