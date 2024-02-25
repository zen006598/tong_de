using System.ComponentModel.DataAnnotations;

namespace tongDe.Models.ViewModels;

public class ClientEditVM
{
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "Mobile phone format error.")]
    public string? Phone { get; set; }
    public string? LineId { get; set; }
    public int ShopId { get; set; }
}
