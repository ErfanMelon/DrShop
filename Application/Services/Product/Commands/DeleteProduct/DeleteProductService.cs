using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Product.Commands.DeleteProduct
{
    public class DeleteProductService : IRequestHandler<RequestDeleteProduct, ResultDto>
    {
        private readonly IDataBaseContext _context;

        public DeleteProductService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto> Handle(RequestDeleteProduct request, CancellationToken cancellationToken)
        {
            var product = _context.Products
                .Include(e => e.ProductFeatures)
                .Include(e => e.ProductImages)
                .Include(e => e.ProductTags)
                .FirstOrDefault(e => e.ProductId == request.productId);
            if (product == null)
                new ThrowThisException(new ArgumentNullException($"Product With Id {request.productId} not found"), "کالا پیدا نشد");

            _context.Products.Remove(product);
            _context.SaveChangesAsync();
            return Task.FromResult(new ResultDto { IsSuccess = true, Message = $"{product.Name} با موفقیت حذف شد" });
        }
    }
    public record RequestDeleteProduct(int productId) : IRequest<ResultDto>;
}
