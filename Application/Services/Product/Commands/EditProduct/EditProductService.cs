using Application.Interfaces;
using Application.Services.Comon.UploadFile;
using Application.Services.Product.Commands.AddProduct;
using Application.Services.Product.Commands.AttachTags;
using Common;
using Domain.Entities.Product;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Commands.EditProduct
{
    public class EditProductService : IRequestHandler<RequestEditProduct, ResultDto>
    {
        private readonly IDataBaseContext _context;
        private readonly IMediator _mediator;

        public EditProductService(IDataBaseContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResultDto> Handle(RequestEditProduct request, CancellationToken cancellationToken)
        {
            var product = _context.Products
                .Include(e => e.ProductFeatures)
                .Include(e => e.ProductImages)
                .Include(e => e.ProductTags)
                .FirstOrDefault(e => e.ProductId == request.ProductId);
            EditProductValidation validationRules = new EditProductValidation(_context);
          var isvalid=  validationRules.Validate(request);
            if (!isvalid.IsValid)
            {
                return new ResultDto { Message = isvalid.Errors[0].ErrorMessage };
            }
            if (product == null)
                new ThrowThisException(new ArgumentNullException($"Product with id {request.ProductId} not found"), "کالا پیدا نشد");
            string productname = product.Name;

            if (product.ProductFeatures.Any())
                _context.ProductFeatures.RemoveRange(product.ProductFeatures);
            if (product.ProductImages.Any())
                _context.ProductImages.RemoveRange(product.ProductImages);
            if (product.ProductTags.Any())
                _context.ProductToTags.RemoveRange(product.ProductTags);

            product.Name = request.ProductName;
            product.ShortDescription = product.ShortDescription;
            product.Description = product.Description;
            product.Category = _context.Categories.Find(request.CategoryId);
            product.Price = request.Price;
            product.ProductFeatures = request.Features.Select(e => new Domain.Entities.Product.ProductFeature
            {
                Feature = e.Feature,
                Value = e.Value
            }).ToList();
            product.Slug = request.ProductSlug;

            product.ProductImages = await ImageUploader(request.Images);
            await _mediator.Send(new RequestAttachTagsToProduct(request.Tags, product.ProductId));

            await _context.SaveChangesAsync();

            return new ResultDto { IsSuccess = true, Message = $"{productname} با موفقیت ویرایش شد" };


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
    public class RequestEditProduct : IRequest<ResultDto>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<FeatureDto> Features { get; set; }
        public List<string> Tags { get; set; }
        public string ProductSlug { get; set; }
    }
    public class EditProductValidation : AbstractValidator<RequestEditProduct>
    {
        private readonly IDataBaseContext _context;
        public EditProductValidation(IDataBaseContext context)
        {
            _context = context;

            RuleFor(e => e.ProductId).NotEmpty().WithMessage("محصولی یافت نشد");
            RuleFor(e => e.ProductName).NotEmpty().WithMessage("نام محصول را وارد کنید");
            RuleFor(e => e.Description).NotEmpty().WithMessage("توضیحات محصول را وارد کنید");
            RuleFor(e => e.ShortDescription).NotEmpty().WithMessage("توضیح کوتاه را وارد کنید");
            RuleFor(e => e.Price).NotEmpty().WithMessage("قیمت را وارد کنید").GreaterThan(0).WithMessage("قیمت وارد شده صحیح نمیباشد");
            RuleFor(e => e.CategoryId).Must(ValidCategory).WithMessage("دسته بندی را به درستی وارد کنید");
            RuleFor(e => e.Images).NotEmpty().WithMessage("تصویر را بارگزاری کنید");
            RuleFor(e => e.Features).NotEmpty().WithMessage("ویژگی محصول را وارد کنید");
            RuleForEach(e => e.Features).SetValidator(new ProductFeatureDtoValidation());
            RuleForEach(e => e.Tags).NotEmpty().WithMessage("تگ را درست وارد کنید").When(e => e.Tags != null);
            RuleFor(e => e.ProductSlug).NotEmpty().WithMessage("اسلاگ محصول را وارد کنید").Matches("^[a-z\\d](?:[a-z\\d_-]*[a-z\\d])?$").WithMessage("اسلاگ را به درستی وارد کنید").Must(UniqueSlug).WithMessage("اسلاگ از قبل موجود است");

        }

        private bool UniqueSlug(string arg)
        {
            return !_context.Products.Any(e => e.Slug == arg);
        }
        private bool ValidCategory(int arg)
        {
            return _context.Categories.Any(e => e.CategoryId == arg);
        }
    }
}
