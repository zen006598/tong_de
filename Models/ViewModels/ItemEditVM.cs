using System.ComponentModel.DataAnnotations;
using tongDe.Models.Validator;

namespace tongDe.Models.ViewModels;

public class ItemEditVM
{
    [Required(ErrorMessage = "Name is required"), UniqueItemName]
    public string? Name { get; set; }
    public string? PinyIn { get; set; }
    [Required]
    public string? Unit { get; set; }
    public int ShopId { get; set; }
}
