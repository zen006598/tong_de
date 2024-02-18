using System.Data;
using Microsoft.EntityFrameworkCore;
using tongDe.Models;

namespace tongDe.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Shop> Shops { get; set; }
}
