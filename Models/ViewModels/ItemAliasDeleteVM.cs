using System.ComponentModel.DataAnnotations;

namespace tongDe.Models.ViewModels;

public class ItemAliasDeleteVM
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
}
