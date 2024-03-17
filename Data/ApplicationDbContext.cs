using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tongDe.Models;

namespace tongDe.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Shop> Shops { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemAlias> ItemAliases { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
}
