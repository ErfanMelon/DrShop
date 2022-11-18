using Application.Interfaces;
using Common;

namespace Application.Services.Account.Queries.GetUsers
{
    public interface IGetUsersService
    {
        ResultDto<UsersDto> Execute(int page, int pagesize);
    }
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    public class UsersDto
    {
        public List<GetUserDto> Users { get; set; }
        public int RowCount { get; set; }
    }
    public class GetUsersService : IGetUsersService
    {
        private readonly IDataBaseContext _context;
        public GetUsersService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<UsersDto> Execute(int page, int pagesize)
        {
            var Users = _context.Users.AsQueryable();
            var result = Users.ToPaged(page, pagesize, out int rowsCount)
                .Select(u => new GetUserDto
                {
                    Email = u.Email,
                    UserId = u.UserId,
                    Username = u.Username,
                    Role = EnumHelpers<BaseRole>.GetDisplayValue((BaseRole)u.RoleId), // first convert roleid to enum then get its name
                });
            // default value for empty result
            result = result.DefaultIfEmpty(new GetUserDto { Email = " - ", Role = " - ", UserId = 0, Username = "داده ای موجود نیست" });
            
            return new ResultDto<UsersDto>
            {
                Data = new UsersDto
                {
                    Users =result.ToList(),
                    RowCount = rowsCount,
                },
                IsSuccess = true,
            };
        }
    }
}
