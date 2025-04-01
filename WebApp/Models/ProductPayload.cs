using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ProductPayload
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
