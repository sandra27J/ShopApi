using System.ComponentModel.DataAnnotations;

namespace ShopApi.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}