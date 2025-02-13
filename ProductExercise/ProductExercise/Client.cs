using System.ComponentModel.DataAnnotations;

namespace ProductExercise;

public class Client
{
    [Key]
    public int ClientId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public ICollection<Address> Addresses { get; set; }
    public ICollection<Order> Orders { get; set; }
}