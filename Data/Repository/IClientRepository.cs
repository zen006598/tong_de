using tongDe.Models;

namespace tongDe.Data.Repository;

public interface IClientRepository : IRepository<Client>
{
    Task<IEnumerable<Client>> GetClients(int shopId);
    Task CancelAsync(int id);
}
