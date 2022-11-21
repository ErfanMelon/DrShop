using Application.Interfaces;
using Common;
using MediatR;

namespace Application.Services.Account.Commands.EditUser
{
    public class EditUserService : IRequestHandler<EditUserDto, ResultDto>
    {
        private readonly IDataBaseContext _context;
        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto> Handle(EditUserDto request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user != null)
            {
                user.Username = request.Username;
                user.Email = request.Email;
                user.RoleId = (int)request.Role;

                //Update Time
                user.UpdateTime = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);
                return new ResultDto { Message = "اطلاعات ویرایش شد", IsSuccess = true };
            }
            return new ResultDto { Message = "کاربر پیدا نشد" };
        }
    }
    public class EditUserDto : IRequest<ResultDto>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BaseRole Role { get; set; }
    }
}
