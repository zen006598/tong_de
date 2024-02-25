using Microsoft.AspNetCore.Identity;

namespace tongDe.Models;

public class ApplicationUser : IdentityUser
{
  public virtual ICollection<Shop> Shops { get; } = new HashSet<Shop>();
}
