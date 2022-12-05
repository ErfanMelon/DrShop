using Application.Interfaces;
using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Commands.EditProduct;
using Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (product != null)
            {
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
                    }).ToList()
                };
                return Task.FromResult(new ResultDto<RequestEditProduct>
                {
                    Data = productDetail,
                    IsSuccess = true
                });
            }
            return Task.FromResult(new ResultDto<RequestEditProduct>
            {
                Message = "محصول پیدا نشد"
            });
        }
    }
    public record RequestGetProductForEdit(int productID) : IRequest<ResultDto<RequestEditProduct>>;
    public class ProductForEditDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public List<FeatureDto> Features { get; set; }
        public List<string> Tags { get; set; }
    }
}
