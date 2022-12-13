using Application.Interfaces;
using Application.Services.Account.Queries.GetUserForEdit;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Account.Queries.GetUserDetail
{
    public class GetUserDetailService : IRequestHandler<RequestGetUser, ResultDto<UserDto>>
    {
        private readonly IDataBaseContext context;

        public GetUserDetailService(IDataBaseContext context)
        {
            this.context = context;
        }

        public Task<ResultDto<UserDto>> Handle(RequestGetUser request, CancellationToken cancellationToken)
        {
            var user = context.Users
                .AsNoTracking()
                .FirstOrDefault(e => e.UserId == request.UserId);
            if (user == null)
                new ThrowThisException(new ArgumentNullException($"User with id {request.UserId} not found "), "کاربری پیدا نشد");
            return Task.FromResult(new ResultDto<UserDto>
            {
                IsSuccess = true,
                Data = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = (BaseRole)user.RoleId,
                    Username = user.Username
                }
            });
        }
    }
    public record RequestGetUser(int UserId) : IRequest<ResultDto<UserDto>>;
}
