using System.ComponentModel.DataAnnotations;
using tongDe.Models.Validator;

namespace tongDe.Models.ViewModels;

public class ItemEditVM
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required"), UniqueItemName]
    public string? Name { get; set; }
    public string? PinyIn { get; set; }
    [Required]
    public string? Unit { get; set; }
    public int ShopId { get; set; }
    public List<ItemAlias>? ItemAliases { get; set; }
}
