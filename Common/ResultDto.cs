using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ResultDto<T>:ResultDto
    {
        public T Data { get; set; }
    }
    public class ResultDto
    {
        public string Messege { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
