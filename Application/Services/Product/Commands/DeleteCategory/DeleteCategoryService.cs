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
                .Include(e=>e.SubCategories)
                .FirstOrDefault(e=>e.CategoryId==request.categoryId);
            if (category != null)
            {
                foreach (Category subcategory in category.SubCategories)
                {
                    subcategory.ParentCategory = null;
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return new ResultDto { IsSuccess = true, Message = $"{category.CategoryName} با موفقیت پاک شد" };
            }
            return new ResultDto { Message ="دسته بندی پیدا نشد"};
        }
    }
}
