using Common;
using MediatR;

namespace Application.Services.Account.Commands.DeleteUser
{
    public record RequestDeleteUser(int UserId) : IRequest<ResultDto>;
}
