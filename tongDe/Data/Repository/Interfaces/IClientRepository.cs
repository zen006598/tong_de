using tongDe.Models;
using tongDe.Data.Repository;

namespace tongDe.Data.Repository.Interfaces;
public interface IClientRepository : IRepository<Client>
{
    Task<IEnumerable<Client>> GetClients(int shopId);
    Task CancelAsync(int id);
}
