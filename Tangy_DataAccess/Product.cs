using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tangy_DataAccess
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public bool ShopFavorites { get; set; }
        public bool CustomerFavorites { get; set; }
        [Required]
        public string? Color { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [Required]
        public Category? Category { get; set; }
    }
}
