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
                .FirstOrDefault(e => e.CategoryId == request.CategoryId);
            if (category == null)
                new ThrowThisException(new ArgumentNullException($"Category with id {request.CategoryId} not found "), "دسته بندی پیدا نشد");
            
            category.ParentCategory = _context.Categories.Find(request.ParentCategory);
            string oldCategoryName = category.CategoryName;
            category.CategoryName = request.CategoryName;
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = $"{oldCategoryName} با موفقیت ویرایش شد" };
        }
    }
}
