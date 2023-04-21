using ECommerceWeb.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class CartItem
    {
        public Products Products { get; set; }
        public int Quantity { get; set; }

    }
}
