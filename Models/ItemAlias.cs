using System.ComponentModel.DataAnnotations;

namespace tongDe.Models;

public class ItemAlias
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}