using Microsoft.EntityFrameworkCore;
using tongDe.Models;
using tongDe.Data.Repository.Interfaces;

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

    public async Task<ItemCategory> GetItemCategoryWithItemsAsync(int id)
    {
        var itemCategory = await _dbContext.ItemCategories.Include(ic => ic.Items).FirstOrDefaultAsync(ic => ic.Id == id);
        return itemCategory;
    }
}
