using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongDe.Services.DTOs.Infos;

public class OrderInfo
{
    [Required]
    public string? ClientName { get; set; }
    public List<OrderItem> OrderItems { get; set; } = null!;
}

public class OrderItem
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [Range(typeof(decimal), "0.001", "9999", ErrorMessage = "Quantity must be greater than 0 and can have up to three decimal places.")]
    [Column(TypeName = "decimal(10,3)")]
    public decimal Quantity { get; set; }
    [Required]
    public string? Unit { get; set; }
}