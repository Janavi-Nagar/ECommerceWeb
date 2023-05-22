using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models.ViewModels
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Please enter Productname")]
        [Display(Name = "Product Name")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        public int Price { get; set; }
        public bool InStock { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProductPicture { get; set; }
        [NotMapped]
        public string? ProductImage { get; set; }
        public Guid ProductCategoryId { get; set; }
        [NotMapped]
        public string? ProductCategoryName { get; set; }
    }
}
