using System.ComponentModel.DataAnnotations;

namespace tongDe.Models;
public class Client
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? LineId { get; set; }
    public bool Cancel { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;
}
