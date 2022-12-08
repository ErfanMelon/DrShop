namespace Domain.Entities.Product
{
    public class ProductToTag
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public ProductTag Tag { get; set; }
        public int TagId { get; set; }
    }
}
