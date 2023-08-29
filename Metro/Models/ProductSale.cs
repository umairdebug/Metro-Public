namespace Metro.Models
{
    public class ProductSale : SharedModel
    {
        public string Title { get; set; }
        public string ShortCode { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
