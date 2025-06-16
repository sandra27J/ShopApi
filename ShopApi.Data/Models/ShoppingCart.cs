using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string User { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}