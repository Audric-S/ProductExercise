using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductExercise;

public class Address
{
    [Key]
    public int AddressId { get; set; }

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

    [ForeignKey("ClientId")]
    [JsonIgnore]
    public Client? Client { get; set; }
}