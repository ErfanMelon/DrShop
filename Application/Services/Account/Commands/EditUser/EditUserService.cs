using Application.Interfaces;
using Common;
using MediatR;

namespace Application.Services.Account.Commands.EditUser
{
    public class EditUserService : IRequestHandler<RequestEditUser, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto> Handle(RequestEditUser request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user != null)
            {
                user.Username = request.Username;
                user.Email = request.Email;
                user.RoleId = (int)request.Role;
                await _context.SaveChangesAsync();

                return new ResultDto { Message = "اطلاعات ویرایش شد", IsSuccess = true };
            }
            return new ResultDto { Message = "کاربر پیدا نشد" };
        }
    }
}
