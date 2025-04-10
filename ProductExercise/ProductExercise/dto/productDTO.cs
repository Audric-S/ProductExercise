using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class ProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int UnitInStock { get; set; }

        public double Weight { get; set; }
    }
}
