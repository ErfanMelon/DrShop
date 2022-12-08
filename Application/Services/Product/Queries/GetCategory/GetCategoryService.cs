using Application.Interfaces;
using Application.Services.Product.Queries.GetCategories;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetCategory
{
    public class GetCategoryService : IRequestHandler<RequestGetCategory, ResultDto<CategoryDto>>
    {
        private readonly IDataBaseContext _context;
        public GetCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<CategoryDto>> Handle(RequestGetCategory request, CancellationToken cancellationToken)
        {
            var category = _context.Categories
                .AsNoTracking()
                .Include(e => e.ParentCategory)
                .Include(e => e.SubCategories)
                .FirstOrDefault(e => e.CategoryId == request.categoryId);
            if (category == null)
                new ThrowThisException(new ArgumentNullException($"Category with id {request.categoryId} not found"), "دسته بندی پیدا نشد");
                CategoryDto categoryDto = new CategoryDto
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    ParentCategory = category.ParentCategory != null ? category.ParentCategory.CategoryName : "ندارد",
                    ParentCategoryId= category.ParentCategory != null ?category.ParentCategory.CategoryId:0,
                    SubCategories = category.SubCategories != null ? category.SubCategories.Select(c => c.CategoryName).ToList() : new List<string>(),
                };
                return Task.FromResult(new ResultDto<CategoryDto> { Data = categoryDto, IsSuccess = true });
            
        }
    }
}
