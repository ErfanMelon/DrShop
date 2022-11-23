using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Product
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Category ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}
