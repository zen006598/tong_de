using System.ComponentModel.DataAnnotations;

namespace tongDe.Models;

public class ItemCategory
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;
    public ICollection<Item> Items { get; } = new List<Item>();
}
