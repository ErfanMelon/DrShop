using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// User's Access Level
    /// </summary>
    public enum BaseRole:int
    {
        [Display(Name ="ادمین")]
        Admin=1,
        [Display(Name = "مشتری")]
        Customer =2
    }
}
