using System.ComponentModel.DataAnnotations;

namespace tongDe.Models.ViewModels;

public class ClientCreateVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [Required]
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "Mobile phone format error.")]
    public string? Phone { get; set; }
    public string? LineId { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = null!;
}
