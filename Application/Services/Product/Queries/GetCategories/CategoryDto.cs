namespace Application.Services.Product.Queries.GetCategories
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        public List<string> SubCategories { get; set; }
    }
}
