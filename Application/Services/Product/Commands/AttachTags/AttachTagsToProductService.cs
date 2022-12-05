using Application.Interfaces;
using Domain.Entities.Product;
using MediatR;

namespace Application.Services.Product.Commands.AttachTags
{
    public class AttachTagsToProductService : IRequestHandler<RequestAttachTagsToProduct>
    {
        private readonly IDataBaseContext _context;

        public AttachTagsToProductService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RequestAttachTagsToProduct request, CancellationToken cancellationToken)
        {
            var product = _context.Products.Find(request.productId);
            if (product != null)
            {
                foreach (string tg in request.tags)
                {
                    ProductTag currentTag;
                    var tag = _context.ProductTags.FirstOrDefault(e=>e.Tag==tg.Trim());
                    if (tag == null)
                    {
                        currentTag = new ProductTag { Tag = tg };
                        _context.ProductTags.Add(currentTag);
                    }
                    else
                    {
                        currentTag = tag;
                    }
                    _context.ProductToTags.Add(new ProductToTag { Product = product, Tag = currentTag });
                }
                await _context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
    public record RequestAttachTagsToProduct(List<string> tags, int productId) : IRequest;
}
