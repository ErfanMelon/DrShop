namespace Domain.Entities.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductToTag> ProductTags { get; set; }
    }
    public class ProductImage
    {
        public string Src { get; set; }
    }
    public class ProductFeature
    {
        public string Feature { get; set; }
        public string Value { get; set; }
    }
    public class ProductTag
    {
        public int TagId { get; set; }
        public string Tag { get; set; }
        public ICollection<ProductToTag> ProductTags { get; set; }
    }
    public class ProductToTag
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ProductTag Tag { get; set; }
        public int TagId { get; set; }
    }
}
