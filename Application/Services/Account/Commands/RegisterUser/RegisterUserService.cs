using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using MediatR;

namespace Application.Services.Account.Commands.RegisterUser
{
    public partial class RegisterUserService : IRequestHandler<RequestRegisterUser, ResultDto<int>>
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<int>> Handle(RequestRegisterUser request, CancellationToken cancellationToken)
        {
            if (_context.Users.Any(u => u.Email == request.Email) == true)
            {
                return new ResultDto<int> { Message = "ایمیل استفاده شده" };
            }

            PasswordHasher hasher = new PasswordHasher();
            User newUser = new User
            {
                Email = request.Email,
                Password = hasher.HashPassword(request.Password),
                RoleId = (int)request.Role,
                Username = request.Username
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return new ResultDto<int> { Data = newUser.UserId, IsSuccess = true, Message = "ثبت نام شدید" };
        }
    }
}
