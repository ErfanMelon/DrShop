using Application.Services.Product.Queries.GetCategories;
using Common;
using MediatR;

namespace Application.Services.Product.Queries.GetCategory
{
    public record RequestGetCategory(int categoryId) : IRequest<ResultDto<CategoryDto>>;
}
