using Common;
using MediatR;

namespace Application.Services.Account.Queries.GetUsers
{
    public record RequestGetUsers(int page, int pagesize) : IRequest<ResultDto<PaginationDto<GetUserDto>>>;
}
