using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// برای ارسال داده بین کنترلر ها و سرویس ها
    /// </summary>
    /// <typeparam name="T">داده قابل نمایش برای کاربر</typeparam>
    public class ResultDto<T>:ResultDto
    {
        public T Data { get; set; }
    }
    /// <summary>
    /// برای ارسال داده بین کنترلر ها و سرویس ها
    /// </summary>
    public class ResultDto
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
