using Microsoft.EntityFrameworkCore;
using tongDe.Models;

namespace tongDe.Data.Repository;

public class ItemCategoryRepository : Repository<ItemCategory>, IItemCategoryRepository
{
    public ItemCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<ItemCategory>> GetItemCategoriesAsync(int shopId)
    {
        return await _dbContext.ItemCategories.Where(ic => ic.ShopId == shopId).ToListAsync();
    }
}
