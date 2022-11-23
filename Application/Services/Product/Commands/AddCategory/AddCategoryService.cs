using Application.Interfaces;
using Common;
using Domain.Entities.Product;
using MediatR;

namespace Application.Services.Product.Commands.AddCategory
{
    public class AddCategoryService : IRequestHandler<RequestAddCategory, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public AddCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Handle(RequestAddCategory request, CancellationToken cancellationToken)
        {
            var parentCategory = _context.Categories.Find(request.parentCategoryId);
            if (parentCategory != null && parentCategory.ParentCategoryId != null)
            {
                return new ResultDto { Message = $"درحال حاضر افزودن دو دسته بندی تودرتو وجود ندارد" };
            }
            Category category = new Category
            {
                CategoryName = request.categoryName,
                ParentCategory = parentCategory
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = $"{request.categoryName} با موفقیت اضافه شد" };
        }
    }
}
