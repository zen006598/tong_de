namespace tongDe.Models.ViewModels;

public class ClientVM
{
    public int ShopId { get; set; }
    public string? ShopName { get; set; }
    public List<Client>? Clients { get; set; }
}
