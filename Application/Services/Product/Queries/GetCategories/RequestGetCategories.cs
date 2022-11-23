using Common;
using MediatR;

namespace Application.Services.Product.Queries.GetCategories
{
    public record RequestGetCategories(int page, int pageSize) : IRequest<ResultDto<PaginationDto<CategoryDto>>>;
}
