using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Account.Queries.GetUsers
{
    public class GetUsersService : IRequestHandler<RequestGetUsers, ResultDto<PaginationDto<GetUserDto>>>
    {
        private readonly IDataBaseContext _context;
        public GetUsersService(IDataBaseContext context)
        {
            _context = context;
        }

        public Task<ResultDto<PaginationDto<GetUserDto>>> Handle(RequestGetUsers request, CancellationToken cancellationToken)
        {
            var Users = _context.Users.AsNoTracking().AsQueryable();
            var result = Users
                .ToPaged(request.page, request.pagesize, out int rowsCount)
                .Select(u => new GetUserDto
                {
                    Email = u.Email,
                    UserId = u.UserId,
                    Username = u.Username,
                    Role = EnumHelpers<BaseRole>.GetDisplayValue((BaseRole)u.RoleId), //  convert roleid to enum and get its name
                })
                .DefaultIfEmpty(new GetUserDto
                { 
                    Email = " - ",
                    Role = " - ",
                    UserId = 0,
                    Username = "داده ای موجود نیست" 
                });// if return null value for empty result


            return Task.FromResult(new ResultDto<PaginationDto<GetUserDto>>
            {
                Data = new PaginationDto<GetUserDto>
                {
                    Items = result.ToList(),
                    PageNumber = request.page,
                    PageSize = request.pagesize,
                    TotalCount = rowsCount
                },
                IsSuccess = true
            });
        }
    }
}
