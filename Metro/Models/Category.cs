using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metro.Models
{
    public class Category : SharedModel
    {
        [Required(ErrorMessage = "Enter category name")]
        [StringLength(100, MinimumLength = 3)]
        [Remote("CategoryNameCheck", "RemoteValidation", AdditionalFields = "Id", ErrorMessage = "Name already exists")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter category name")]
        [StringLength(100, MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Logo { get; set; }
        public string LogoUrl { get; set; }    

        [Required]
        public int Rank { get; set; }
        public CategoryStatus Status { get; set; }
        public CategoryType Type { get; set; }


        public string ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public string AppUsersID { get; set; }
        public virtual AppUser AppUsers { get; set; }


        public virtual List<Category> Childs { get; set; }

        [InverseProperty("Category")]
        public List<Product> CategoryWiseProducts { get; set; }

        [InverseProperty("Brand")]
        public List<Product> BrandWiseProducts { get; set; }
    }
    public class CategoryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public string Logo { get; set; }
        public string LogoUrl { get; set; }
        public string Parent { get; set; }
        public int CategoryWiseProducts { get; set; }
        public int BrandWiseProducts { get; set; }
        public CategoryStatus Status { get; set; }
        public CategoryType Type { get; set; }
    }
    public enum CategoryStatus
    {
        UnderVerification = 0,
        Active = 10,
        Upcoming = 20,
        Deactivated = 30
    }
    public enum CategoryType
    {
        Category = 0,
        Brand = 10,
        Blogs = 20
    }
}
