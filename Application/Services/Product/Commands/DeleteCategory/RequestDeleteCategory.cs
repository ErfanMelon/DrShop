using Common;
using MediatR;

namespace Application.Services.Product.Commands.DeleteCategory
{
    public record RequestDeleteCategory(int categoryId) : IRequest<ResultDto>;
}
