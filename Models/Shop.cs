using System.ComponentModel.DataAnnotations;

namespace tongDe.Models;

public class Shop
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required!")]
    public string? Name { get; set; }
    public DateTime? DeleteTime { get; set; }
    public string UserId { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public ICollection<Client> Clients { get; } = new List<Client>();
}
