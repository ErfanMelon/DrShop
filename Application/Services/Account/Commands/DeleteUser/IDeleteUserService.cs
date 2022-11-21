using Application.Interfaces;
using Common;
using MediatR;

namespace Application.Services.Account.Commands.DeleteUser
{
    public record RequestDeleteUser(int UserId) : IRequest<ResultDto>;
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
            if (user != null)
            {
                user.RemoveTime = DateTime.Now;
                user.IsRemoved = true;
                await _context.SaveChangesAsync(cancellationToken);
                return new ResultDto { IsSuccess = true, Message = "کاربر حذف شد" };
            }
            return new ResultDto { Message = "کاربر پیدا نشد" };
        }
    }
}
