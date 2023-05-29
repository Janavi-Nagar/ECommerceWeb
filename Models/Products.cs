using ECommerceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class Products
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Please enter Productname")]
        [Display(Name = "Product Name")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        public int Price { get; set; }
        public bool InStock { get; set; }

        //[Required(ErrorMessage = "Please choose profile image")]
        public string ProductPicture { get; set; }

        [Display(Name = "Product Category")]
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}
