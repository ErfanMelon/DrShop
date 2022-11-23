using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetCategories
{
    public class GetCategoriesService : IRequestHandler<RequestGetCategories, ResultDto<PaginationDto<CategoryDto>>>
    {
        private readonly IDataBaseContext _context;
        public GetCategoriesService(IDataBaseContext context)
        {
            _context = context;
        }
        public Task<ResultDto<PaginationDto<CategoryDto>>> Handle(RequestGetCategories request, CancellationToken cancellationToken)
        {
            var categories = _context.Categories
                .AsNoTracking()
                .Include(e => e.ParentCategory)
                .Include(e => e.SubCategories)
                .ToPaged(request.page, request.pageSize, out int rowsCount)
                .Select(e => new CategoryDto
                {
                    CategoryId = e.CategoryId,
                    CategoryName = e.CategoryName,
                    ParentCategory = e.ParentCategory!=null? e.ParentCategory.CategoryName:" - ",
                    SubCategories = e.SubCategories.Any()? e.SubCategories.Select(c => c.CategoryName).ToList():new List<string> { " - " }
                })
                .DefaultIfEmpty(new CategoryDto
                {
                    CategoryId = 0,
                    CategoryName = "داده ای موجود نیست",
                    ParentCategory = " - ",
                    SubCategories = new List<string> { " - " }
                });
            return Task.FromResult(new ResultDto<PaginationDto<CategoryDto>>
            {
                Data = new PaginationDto<CategoryDto>
                {
                    Items = categories.ToList(),
                    PageNumber = request.page,
                    PageSize = request.pageSize,
                    TotalCount = rowsCount
                },
                IsSuccess = true
            }
            );
        }
    }
}
