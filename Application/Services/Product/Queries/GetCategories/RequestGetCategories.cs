using Common;
using MediatR;

namespace Application.Services.Product.Queries.GetCategories
{
    public record RequestGetCategories(int page, int pageSize=int.MaxValue, string SearchKey="") : IRequest<ResultDto<PaginationDto<CategoryDto>>>;
}
