using System.ComponentModel.DataAnnotations;
using tongDe.Models;

namespace tongDe.Services.DTOs.Infos;

public class OrderInfo
{
    [Required]
    public string? ClientName { get; set; }
    public List<OrderItem> OrderItems { get; set; } = null!;
}