using System.ComponentModel.DataAnnotations;

namespace tongDe.Models.ViewModels;

public class ItemCategoryEditVM
{
    [Required]
    public string? Name { get; set; }
    public int ShopId { get; set; }
}
