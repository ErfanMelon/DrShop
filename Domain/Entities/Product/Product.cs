namespace Domain.Entities.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Visits { get; set; }
        public DateTime InsertTime { get; set; }
        public string Slug  { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductToTag> ProductTags { get; set; }
    }
}
