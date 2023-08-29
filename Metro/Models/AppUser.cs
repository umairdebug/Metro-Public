using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metro.Models
{
    public class AppUser : SharedModel
    {
        [Required(ErrorMessage = "Minimum 3 letters are required")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email")]
        [Remote("CategoryNameCheck", "RemoteValidation", AdditionalFields = "Id", ErrorMessage = "Name already exists")]
        public string Email { get; set; }
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        public string EncryptedPassword { get; set; }
        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        public string ConformPassword { get; set; }

        public virtual List<AppRole> Roles { get; set; }
        public virtual List<Category> Category { get; set; }
        public virtual List<Product> Product { get; set; }
        public virtual List<Order> Order { get; set; }

    }

    public class AppRole : SharedModel
    {
        [Required(ErrorMessage = "Minimum 3 letters are required")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        public List<AppUser> User { get; set; }
    }

    public class LoginViewModel : SharedModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class AppUserViewModel : SharedModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
