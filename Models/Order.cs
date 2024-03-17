using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using tongDe.Data;

namespace tongDe.Models;

public class Order : IEntity
{
    public int Id { get; set; }
    [Required]
    public string? ClientName { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    public int ShopId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Shop Shop { get; set; } = null!;
    public int ClientId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Client Client { get; set; } = null!;

}