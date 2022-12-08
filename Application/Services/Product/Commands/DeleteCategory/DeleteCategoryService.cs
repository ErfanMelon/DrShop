using Application.Interfaces;
using Common;
using Domain.Entities.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Commands.DeleteCategory
{
    public class DeleteCategoryService : IRequestHandler<RequestDeleteCategory, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public DeleteCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Handle(RequestDeleteCategory request, CancellationToken cancellationToken)
        {
            var category = _context.Categories
                .Include(e => e.SubCategories)
                .Include(e=>e.Products)
                .FirstOrDefault(e => e.CategoryId == request.categoryId);
            if (category == null)
                new ThrowThisException(new ArgumentNullException($"Category With Id {request.categoryId} not exist"), "دسته بندی پیدا نشد");

            if (category.Products.Any())
                return new ResultDto { Message = $"این دسته بندی {category.Products.Count()} محصول دارد قبل از حذف آن محصول را ویرایش کنید" };
            foreach (Category subcategory in category.SubCategories)
            {
                subcategory.ParentCategory = null;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = $"{category.CategoryName} با موفقیت پاک شد" };
        }
    }
}
