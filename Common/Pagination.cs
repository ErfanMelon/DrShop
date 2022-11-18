using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    ///کلاسی برای صفحه بندی داده ها
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// داده های ورودی را صفحه بندی میکند
        /// </summary>
        /// <typeparam name="TSource">نوع داده ورودی</typeparam>
        /// <param name="source">داده ورودی</param>
        /// <param name="page">شماره صفحه</param>
        /// <param name="pageSize">تعداد داده ها در هر صفحه</param>
        /// <param name="rowsCount">تعداد کل داده های موجود</param>
        /// <returns></returns>
        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int pageSize, out int rowsCount)
        {
            rowsCount = source.Count();
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
