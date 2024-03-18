using Microsoft.EntityFrameworkCore;
using tongDe.Models;
using tongDe.Data.Repository.Interfaces;

namespace tongDe.Data.Repository;
public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Item>> GetItems(int shopId)
    {
        var items = await _dbContext.Items
            .Where(i => i.ShopId == shopId)
            .ToListAsync();
        return items;
    }

    public async Task<Item> GetItemWithAliasesAsync(int id)
    {
        return await _dbContext.Items
                               .Include(i => i.ItemAliases)
                               .FirstOrDefaultAsync(i => i.Id == id);
    }
}
