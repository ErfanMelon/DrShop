using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Account
{
    /// <summary>
    /// کاربر
    /// </summary>
    public class User:BaseEntity
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        [StringLength(50,MinimumLength =3)]
        public string Username { get; set; }
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// گذرواژه کاربر
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// سطح دسترسی کاربر
        /// </summary>
        public virtual Role Role { get; set; }
        /// <summary>
        /// شناسه سطح دسترسی کاربر
        /// </summary>
        public int RoleId { get; set; }
    }
}
