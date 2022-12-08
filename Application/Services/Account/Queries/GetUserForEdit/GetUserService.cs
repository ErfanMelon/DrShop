using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Account.Queries.GetUserForEdit
{
    public class GetUserService : IRequestHandler<RequestGetUser, ResultDto<UserDto>>
    {
        private readonly IDataBaseContext _context;
        public GetUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<UserDto>> Handle(RequestGetUser request, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(e => e.UserId == request.UserId);
            if (user == null)
                new ThrowThisException(new ArgumentNullException($"User with id {request.UserId} not Found"), "کاربر یافت نشد");

            UserDto resultUser = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = (BaseRole)user.RoleId,
                Username = user.Username
            };
            return Task.FromResult(new ResultDto<UserDto> { Data = resultUser, IsSuccess = true });
        }
    }
}
