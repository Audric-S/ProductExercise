using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

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

    public ICollection<OrderDetails> OrderDetails { get; set; }
}