using Application.Interfaces;
using Application.Services.Comon.UploadFile;
using Application.Services.Product.Commands.AttachTags;
using Common;
using Domain.Entities.Product;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Product.Commands.AddProduct
{
    public class AddProductService : IRequestHandler<RequestAddProduct, ResultDto<int>>
    {
        private readonly IDataBaseContext _context;
        private readonly IMediator _mediator;
        public AddProductService(IDataBaseContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResultDto<int>> Handle(RequestAddProduct request, CancellationToken cancellationToken)
        {
            AddProductValidation validationRules = new AddProductValidation(_context);
            var validrequest = validationRules.Validate(request);
            if (!validrequest.IsValid)
            {
                return new ResultDto<int> { Message = validrequest.Errors[0].ErrorMessage };
            }
            Domain.Entities.Product.Product newProduct = new Domain.Entities.Product.Product
            {
                Category = _context.Categories.Find(request.CategoryId),
                Description = request.Description,
                Name = request.ProductName,
                Price = request.Price,
                ShortDescription = request.ShortDescription,
                ProductFeatures = request.Features.Select(e => new ProductFeature
                {
                    Feature = e.Feature,
                    Value = e.Value
                }).ToList(),
                Visits=0,
            };
            _context.Products.Add(newProduct);

            newProduct.ProductImages = await ImageUploader(request.Images);

            await _context.SaveChangesAsync();

            await _mediator.Send(new RequestAttachTagsToProduct(request.Tags, newProduct.ProductId));

            return new ResultDto<int> { Data = newProduct.ProductId, IsSuccess = true, Message = $"{newProduct.Name} با موفقیت اضافه شد" };
        }

        private async Task<List<ProductImage>> ImageUploader(List<IFormFile> images)
        {
            List<ProductImage> productImages = new List<ProductImage>();
            foreach (FormFile image in images)
            {
                var result = await _mediator.Send(new RequestUploadFile(image));
                if (result.IsSuccess)
                {
                    productImages.Add(new ProductImage
                    {
                        Src = result.Data
                    });
                }
            }
            return productImages;
        }
    }
    public class RequestAddProduct : IRequest<ResultDto<int>>
    {
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<FeatureDto> Features { get; set; }
        public List<string> Tags { get; set; }
    }
    public class FeatureDto
    {
        public string Feature { get; set; }
        public string Value { get; set; }
    }
    public class AddProductValidation : AbstractValidator<RequestAddProduct>
    {
        private readonly IDataBaseContext _context;
        public AddProductValidation(IDataBaseContext context)
        {
            _context = context;

            RuleFor(e => e.ProductName).NotEmpty().WithMessage("نام محصول را وارد کنید");
            RuleFor(e => e.Description).NotEmpty().WithMessage("توضیحات محصول را وارد کنید");
            RuleFor(e => e.ShortDescription).NotEmpty().WithMessage("توضیح کوتاه را وارد کنید");
            RuleFor(e => e.Price).NotEmpty().WithMessage("قیمت را وارد کنید").GreaterThan(0).WithMessage("قیمت وارد شده صحیح نمیباشد");
            RuleFor(e => e.CategoryId).Must(ValidCategory).WithMessage("دسته بندی را به درستی وارد کنید");
            RuleFor(e => e.Images).NotEmpty().WithMessage("تصویر را بارگزاری کنید");
            RuleFor(e => e.Features).NotEmpty().WithMessage("ویژگی محصول را وارد کنید");
            RuleForEach(e => e.Features).SetValidator(new ProductFeatureDtoValidation());
            RuleForEach(e => e.Tags).NotEmpty().WithMessage("تگ را درست وارد کنید").When(e => e.Tags != null);

        }


        private bool ValidCategory(int arg)
        {
            return _context.Categories.Any(e => e.CategoryId == arg);
        }
    }
    public class ProductFeatureDtoValidation : AbstractValidator<FeatureDto>
    {
        public ProductFeatureDtoValidation()
        {
            RuleFor(e => e.Feature).NotEmpty().WithMessage("ویژگی نمیتواند خالی باشد");
            RuleFor(e => e.Value).NotEmpty().WithMessage("مقدار ویژگی نمیتواند خالی باشد");
        }
    }
}
