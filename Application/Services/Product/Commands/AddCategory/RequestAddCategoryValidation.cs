using FluentValidation;

namespace Application.Services.Product.Commands.AddCategory
{
    public class RequestAddCategoryValidation : AbstractValidator<RequestAddCategory>
    {
        public RequestAddCategoryValidation()
        {
            RuleFor(e => e.categoryName).NotEmpty().WithMessage("نام دسته بندی را وارد کنید");
        }
    }
}
