using System.ComponentModel.DataAnnotations;
using tongDe.Data;

namespace tongDe.Models;

public class OrderItem : IEntity
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public string? Unit { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
