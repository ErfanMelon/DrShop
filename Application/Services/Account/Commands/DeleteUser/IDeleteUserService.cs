using Application.Interfaces;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account.Commands.DeleteUser
{
    public interface IDeleteUserService
    {
        ResultDto Execute(int userId);
    }
    public class DeleteUserService: IDeleteUserService
    {
        private readonly IDataBaseContext _context;
        public DeleteUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user!=null)
            {
                user.RemoveTime = DateTime.Now;
                user.IsRemoved = true;
                _context.SaveChanges();
                return new ResultDto { IsSuccess = true, Message = "کاربر حذف شد" };
            }
            return new ResultDto { Message = "کاربر پیدا نشد" };
        }
    }
}
