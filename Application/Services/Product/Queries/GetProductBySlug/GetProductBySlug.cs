using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProductBySlug
{
    public class GetProductBySlug : IRequestHandler<RequestGetProductBySlug, ResultDto<ResultGetProduct>>
    {
        private readonly IDataBaseContext _context;

        public GetProductBySlug(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<ResultGetProduct>> Handle(RequestGetProductBySlug request, CancellationToken cancellationToken)
        {
            var product = _context.Products
                .Include(e => e.ProductImages)
                .Include(e => e.Category)
                .Include(e => e.ProductFeatures)
                .FirstOrDefault(e => e.Slug == request.slug);
            if (product == null)
            {
                new ThrowThisException(new ArgumentNullException($"Product with slug {request.slug} not found"), "محصول پیدا نشد");
            }
            product.Visits++;
            await _context.SaveChangesAsync();
            return new ResultDto<ResultGetProduct>
            {
                IsSuccess = true,
                Data = new ResultGetProduct
                {
                    Category = product.Category.CategoryName,
                    Description = product.Description,
                    Features = String.Join(" - ", product.ProductFeatures.Select(e =>
                        $"{e.Feature} : {e.Value}"
                    ).ToList()),
                    Price = product.Price,
                    ProductId = product.ProductId,
                    Image = product.ProductImages?.FirstOrDefault()?.Src ?? "",
                    ProductName = product.Name
                }
            };

        }
    }
    public record RequestGetProductBySlug(string slug) : IRequest<ResultDto<ResultGetProduct>>;
    public class ResultGetProduct
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string Category { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    }

}
