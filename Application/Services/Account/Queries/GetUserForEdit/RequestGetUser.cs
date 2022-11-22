using Common;
using MediatR;

namespace Application.Services.Account.Queries.GetUserForEdit
{
    public record RequestGetUser(int UserId) : IRequest<ResultDto<UserDto>>;
}
