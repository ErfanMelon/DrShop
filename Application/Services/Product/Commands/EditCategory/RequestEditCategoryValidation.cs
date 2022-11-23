using FluentValidation;

namespace Application.Services.Product.Commands.EditCategory
{
    public class RequestEditCategoryValidation:AbstractValidator<RequestEditCategory>
    {
        public RequestEditCategoryValidation()
        {
            RuleFor(e => e.CategoryName).NotEmpty().WithMessage("نام دسته بندی را وارد کنید");
            RuleFor(e => e.CategoryId).NotEmpty().WithMessage("دسته بندی پیدا نشد");
        }
    }
}
