using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// DataTransfareObject for exchange data between classes and controllers
    /// </summary>
    /// <typeparam name="T">Data</typeparam>
    public class ResultDto<T>:ResultDto
    {
        public T Data { get; set; }
    }
    /// <summary>
    ///  DataTransfareObject for result data exchange between classes and controllers
    /// </summary>
    public class ResultDto
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
