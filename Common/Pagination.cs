using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Pagination
    {
        /// <summary>
        /// Paginating big data to multiple page
        /// </summary>
        /// <typeparam name="TSource">type of enumrator</typeparam>
        /// <param name="source">data</param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">number of all records in page</param>
        /// <param name="rowsCount">all data count</param>
        /// <returns></returns>
        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int pageSize, out int rowsCount)
        {
            rowsCount = source.Count();
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
    /// <summary>
    /// DataTransfareObject for pagination
    /// </summary>
    /// <typeparam name="TList">item in list</typeparam>
    public class PaginationDto<TList>
    {
        public List<TList> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
