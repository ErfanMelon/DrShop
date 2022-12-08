using Application.Interfaces;
using Common;
using MediatR;

namespace Application.Services.Account.Commands.DeleteUser
{
    public class DeleteUserService : IRequestHandler<RequestDeleteUser, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public DeleteUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Handle(RequestDeleteUser request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                new ThrowThisException(new ArgumentNullException($"User with id {request.UserId} not Found"), "کاربر یافت نشد");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = "کاربر حذف شد" };

        }
    }
}
