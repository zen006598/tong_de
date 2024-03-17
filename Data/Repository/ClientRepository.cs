using Microsoft.EntityFrameworkCore;
using tongDe.Models;
using tongDe.Data.Repository.Interfaces;

namespace tongDe.Data.Repository;

public class ClientRepository : Repository<Client>, IClientRepository
{

    public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task CancelAsync(int id)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client is not null) _dbContext.Clients.Remove(client);
    }


    public async Task<IEnumerable<Client>> GetClients(int shopId)
    {
        var clients = await _dbContext.Clients.Where(c => c.ShopId == shopId && !c.Cancel).ToListAsync();
        return clients;
    }
}
