using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class BaseEntity
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
        public DateTime? RemoveTime { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
