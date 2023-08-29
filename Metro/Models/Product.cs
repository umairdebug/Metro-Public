using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metro.Models
{
    public class Product : SharedModel
    {
        [Required(ErrorMessage = "Enter category name")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [StringLength(100, MinimumLength = 3)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int Rank { get; set; }

        [Required(ErrorMessage = "Enter PurchasePrice")]
        [Range(0, 300)]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Enter SalePrice")]
        [Range(0, 300)]
        public decimal SalePrice { get; set; }


        public string AppUsersId { get; set; }
        public virtual AppUser AppUsers { get; set; }

        [ForeignKey("CategoryId")]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("Brand")]
        public string BrandId { get; set; }
        public virtual Category Brand { get; set; }

        public virtual List<ProductImage> Images { get; set; }
        public virtual List<ProductDisplaySection> DisplaySections { get; set; }
        public virtual List<ProductSale> Sales { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

        [NotMapped]
        public List<IFormFile> Uploads { get; set; }
    }

    public class ProductImage : SharedModel
    {
        public string Url { get; set; }
        public int Rank { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class ProductDisplaySection : SharedModel
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Rank { get; set; }
        public string ImageUrl { get; set; }

        public virtual List<Product> Products { get; set; }
    }
    public class ProductViewModel : SharedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Image { get; set; }
    }
}
