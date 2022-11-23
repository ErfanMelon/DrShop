using Common;
using MediatR;

namespace Application.Services.Product.Commands.AddCategory
{
    public record RequestAddCategory(string categoryName, int parentCategoryId) : IRequest<ResultDto>;
}
