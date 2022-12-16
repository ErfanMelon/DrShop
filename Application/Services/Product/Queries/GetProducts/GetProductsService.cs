using Application.Interfaces;
using Application.Services.Product.Queries.GetProductStars;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProducts
{
    public class GetProductsService : IRequestHandler<RequestGetProducts, ResultDto<PaginationDto<ProductListDto>>>
    {
        private readonly IDataBaseContext _context;
        private readonly IMediator _mediator;

        public GetProductsService(IDataBaseContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public Task<ResultDto<PaginationDto<ProductListDto>>> Handle(RequestGetProducts request, CancellationToken cancellationToken)
        {
            var products = _context.Products
                .Include(e => e.ProductImages)
                .AsNoTracking()
                .ToPaged(request.page, request.pagesize, out int rows)
                .Select(e => new ProductListDto
                {
                    Image = e.ProductImages.FirstOrDefault()?.Src ?? "",
                    Price = e.Price,
                    ProductId = e.ProductId,
                    ProductName = e.Name,
                    Stars = 0
                }).ToList()
                .DefaultIfEmpty(new ProductListDto { Image = "", Price = 0, ProductId = 0, ProductName = "بدون محصول" });
            foreach (var item in products)
            {
                item.Stars = _mediator.Send(new RequestGetProductStars(item.ProductId)).Result;
            }
            return Task.FromResult(new ResultDto<PaginationDto<ProductListDto>>
            {
                IsSuccess = true,
                Data = new PaginationDto<ProductListDto>
                {
                    Items = products.ToList(),
                    PageNumber = request.page,
                    PageSize = request.pagesize,
                    TotalCount = rows
                }
            });
        }
    }
    public class ProductListDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Stars { get; set; }
    }
    public record RequestGetProducts(int page, int pagesize) : IRequest<ResultDto<PaginationDto<ProductListDto>>>;
}
