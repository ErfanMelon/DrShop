using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account.Commands.EditUser
{
    public interface IEditUserService
    {
        ResultDto Execute(RequestEditUserDto request);
    }
    public class EditUserService : IEditUserService
    {
        private readonly IDataBaseContext _context;
        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditUserDto request)
        {
            var user = _context.Users.Find(request.UserId);
            if (user != null)
            {
                user.Username = request.Username;
                user.Email = request.Email;
                user.RoleId = request.RoleId;

                //Update Time
                user.UpdateTime = DateTime.Now;

                _context.SaveChanges();
                return new ResultDto { Message = "اطلاعات ویرایش شد", IsSuccess = true };
            }
            return new ResultDto { Message = "کاربر پیدا نشد" };
        }
    }
    public class RequestEditUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
