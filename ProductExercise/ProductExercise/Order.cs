using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductExercise;

[Table(nameof(Order))]
public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; }
}