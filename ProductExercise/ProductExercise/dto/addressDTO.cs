using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class AddressDto
    {
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
