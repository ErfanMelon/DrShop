using Application.Interfaces;
using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Commands.EditProduct;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProduct
{
    public class GetProductForEdit : IRequestHandler<RequestGetProductForEdit, ResultDto<RequestEditProduct>>
    {
        private readonly IDataBaseContext _context;

        public GetProductForEdit(IDataBaseContext context)
        {
            _context = context;
        }
        public Task<ResultDto<RequestEditProduct>> Handle(RequestGetProductForEdit request, CancellationToken cancellationToken)
        {
            var product = _context.Products
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.ProductFeatures)
                .Include(e => e.ProductTags)
                .ThenInclude(e => e.Tag)
                .FirstOrDefault(e => e.ProductId == request.productID);
            if (product == null)
                new ThrowThisException(new ArgumentNullException($"product with id {request.productID} not found"), "محصول پیدا نشد");

            RequestEditProduct productDetail = new RequestEditProduct
                {
                    CategoryId = product.Category.CategoryId,
                    Description = product.Description,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    ShortDescription = product.ShortDescription,
                    Tags = product.ProductTags.Select(e => e.Tag.Tag).ToList(),
                    Features = product.ProductFeatures.Select(e => new FeatureDto
                    {
                        Feature = e.Feature,
                        Value = e.Value
                    }).ToList(),
                    ProductSlug=product.Slug
                };
                return Task.FromResult(new ResultDto<RequestEditProduct>
                {
                    Data = productDetail,
                    IsSuccess = true
                });
        }
    }
    public record RequestGetProductForEdit(int productID) : IRequest<ResultDto<RequestEditProduct>>;
}
