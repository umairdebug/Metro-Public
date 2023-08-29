using System.ComponentModel.DataAnnotations;

namespace Metro.Models
{
    public class Order : SharedModel
    {
        public string Email { get; set; }
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "The ShippingAddress field is required.")]
        [StringLength(500)]
        public string ShippingAddress { get; set; }

        public string AppUsersID { get; set; }
        public virtual AppUser AppUsers { get; set; }

        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
    public class OrderDetail : SharedModel
    {
        public int QuantityDemanded { get; set; }
        public int? QuantitySent { get; set; }

        [Required]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public string ProductId { get; set; }
        public Product Product { get; set; } 
    }
}
