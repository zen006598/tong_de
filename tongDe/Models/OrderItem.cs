using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tongDe.Data;

namespace tongDe.Models;

public class OrderItem : IEntity
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    [Range(typeof(decimal), "0.001", "9999", ErrorMessage = "Quantity must be greater than 0 and can have up to three decimal places.")]
    [Column(TypeName = "decimal(10,3)")]
    public decimal Quantity { get; set; }
    [Required]
    public string? Unit { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
