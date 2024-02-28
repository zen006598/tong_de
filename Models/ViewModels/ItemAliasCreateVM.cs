using System.ComponentModel.DataAnnotations;
using tongDe.Models.Validator;

namespace tongDe.Models.ViewModels;

public class ItemAliasCreateVM
{
    public int Id { get; set; }
    [Required, UniqueItemAliasName]
    public string? Name { get; set; }
    public int ItemId { get; set; }
}