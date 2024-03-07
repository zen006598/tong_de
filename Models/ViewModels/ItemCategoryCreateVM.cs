using System.ComponentModel.DataAnnotations;

namespace tongDe.Models.ViewModels;

public class ItemCategoryCreateVM
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public int ShopId { get; set; }
}
