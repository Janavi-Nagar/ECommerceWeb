using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class ProductCategory
    {
        public Guid ProductCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}