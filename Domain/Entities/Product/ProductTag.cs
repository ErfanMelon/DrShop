namespace Domain.Entities.Product
{
    public class ProductTag
    {
        public int TagId { get; set; }
        public string Tag { get; set; }
        public ICollection<ProductToTag> ProductTags { get; set; }
    }
}
