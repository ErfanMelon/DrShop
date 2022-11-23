using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetCategory
{
    public class GetParentCategoriesService : IRequestHandler<RequestGetParentCategories, SelectList>
    {
        private readonly IDataBaseContext _context;
        public GetParentCategoriesService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<SelectList> Handle(RequestGetParentCategories request, CancellationToken cancellationToken)
        {
            var parentCategories = _context.Categories
                .AsNoTracking()
                .Where(e => e.ParentCategoryId == null)
                .Select(e =>
                new SelectListItem
                {
                    Text = e.CategoryName,
                    Value = e.CategoryId.ToString(),
                })
                .ToList();
            parentCategories.Add(new SelectListItem { Text = " بدون دسته بندی والد ", Value = "0" });
            parentCategories.Reverse();
            return Task.FromResult(new SelectList(parentCategories, "Value", "Text"));
        }
    }
}
