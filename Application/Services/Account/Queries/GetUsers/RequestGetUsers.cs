using Common;
using MediatR;

namespace Application.Services.Account.Queries.GetUsers
{
    public record RequestGetUsers(int page, int pagesize,string? SearchKey) : IRequest<ResultDto<PaginationDto<GetUserDto>>>;
}
