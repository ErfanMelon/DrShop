using Application.Interfaces;
using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Queries.GetProductStars;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProduct
{
    public class GetProductService : IRequestHandler<RequestGetProduct, ResultDto<ProductDetailDto>>
    {
        private readonly IDataBaseContext _context;
        private readonly IMediator _mediator;

        public GetProductService(IDataBaseContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public Task<ResultDto<ProductDetailDto>> Handle(RequestGetProduct request, CancellationToken cancellationToken)
        {
            var product = _context.Products
                .Include(e => e.ProductImages)
                .Include(e => e.ProductFeatures)
                .Include(e => e.ProductTags)
                .ThenInclude(e => e.Tag)
                .Include(e => e.Comments)
                .ThenInclude(e => e.Disadvantages)
                .Include(e => e.Comments)
                .ThenInclude(e => e.Advantages)
                .Include(e => e.Comments)
                .ThenInclude(e => e.User)
                .AsNoTracking()
                .FirstOrDefault(e => e.ProductId == request.productId);
            if (product == null)
                new ThrowThisException(new ArgumentNullException($"Product With Id {request.productId} not found"), "کالا پیدا نشد");

            ProductDetailDto productDetail = new ProductDetailDto
            {
                AllFeatures = product.ProductFeatures.Select(e => new FeatureDto { Feature = e.Feature, Value = e.Value }).ToList(),
                TopFeatures = product.ProductFeatures.Take(3).Select(e => new FeatureDto { Feature = e.Feature, Value = e.Value }).ToList(),
                LongDescription = product.Description,
                Price = product.Price,
                ProductId = product.ProductId,
                ProductName = product.Name,
                ShortDescription = product.ShortDescription,
                Images = product.ProductImages?.Skip(1)?.Select(e => e.Src)?.ToList() ?? new List<string> { "" },
                MainImage = product.ProductImages?.FirstOrDefault()?.Src ?? "",
                Tags = product.ProductTags.Select(e => e.Tag.Tag).ToList(),
                Inventory = 0, // Not Implemented Yet
                Slug = product.Slug,
            };
            productDetail.Comments = product.Comments?.Select(e => new CommentDto
            {
                Adv = e.Advantages.Select(a => a.Point).ToList(),
                DisAdv = e.Disadvantages.Select(a => a.Point).ToList(),
                Comment = e.CommentBody,
                CommentId = e.CommentId,
                PostDate = e.InsertTime,
                UserId = e.User.UserId,
                Username = e.User.Username
            }).ToList() ?? null;
            productDetail.Stars = _mediator.Send(new RequestGetProductStars(request.productId)).Result;

            return Task.FromResult(new ResultDto<ProductDetailDto>
            {
                IsSuccess = true,
                Data = productDetail
            });

        }
    }
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Price { get; set; }
        public string MainImage { get; set; }
        public List<string> Images { get; set; }
        public List<FeatureDto> AllFeatures { get; set; }
        public List<FeatureDto> TopFeatures { get; set; }
        public List<string> Tags { get; set; }
        public int Inventory { get; set; }
        public int Stars { get; set; }
        public string Slug { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public List<string> Adv { get; set; }
        public List<string> DisAdv { get; set; }
        public string Comment { get; set; }
        public DateTime PostDate { get; set; }
        public string Username { get; set; }
    }
    public record RequestGetProduct(int productId) : IRequest<ResultDto<ProductDetailDto>>;
}
