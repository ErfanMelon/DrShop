using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Commands.EditCategory
{
    public class EditCategoryService : IRequestHandler<RequestEditCategory, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public EditCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Handle(RequestEditCategory request, CancellationToken cancellationToken)
        {
            var category = _context.Categories
                .Include(e => e.ParentCategory)
                .Include(e=>e.SubCategories)
                .FirstOrDefault(e => e.CategoryId == request.CategoryId);
            if (category == null)
                new ThrowThisException(new ArgumentNullException($"Category with id {request.CategoryId} not found "), "دسته بندی پیدا نشد");

            var parentcategory = _context.Categories
                .Include(e => e.SubCategories)
                .FirstOrDefault(e => e.CategoryId == request.ParentCategory);

            if (parentcategory != null && category.SubCategories.Any())
                new ThrowThisException(new ArgumentOutOfRangeException($"CategoryTree set to one branch {parentcategory.CategoryName} cannot nest with {category.CategoryName}"), "درحال حاضر افزودن دو دسته بندی تودرتو وجود ندارد");

            category.ParentCategory = parentcategory;
            string oldCategoryName = category.CategoryName;
            category.CategoryName = request.CategoryName;
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = $"{oldCategoryName} با موفقیت ویرایش شد" };
        }
    }
}
