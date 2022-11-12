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
    /// سطح دسترسی کاربران سایت به بخش های مختلف
    /// </summary>
    public class Role:BaseEntity
    {
        /// <summary>
        /// شناسه سطح دسترسی
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// نام سطح دسترسی
        /// </summary>
        [MaxLength(20)]
        public string AccessLevel { get; set; }
    }
}
