using System.ComponentModel.DataAnnotations;

namespace ShopApi.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }
}