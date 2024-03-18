using Microsoft.EntityFrameworkCore;
using tongDe.Models;
using tongDe.Data.Repository.Interfaces;

namespace tongDe.Data.Repository;
public class ShopRepository : Repository<Shop>, IShopRepository
{
    public ShopRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    //TODO: Shop Cancel Method
    public Task CancelAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Shop>> GetShops(string userId)
    {
        var shops = await _dbContext.Shops.Where(s => s.UserId == userId).ToListAsync();
        return shops;
    }

    public async Task<Shop> GetShopWithItemCategoriesAsync(int id)
    {
        return await _dbContext.Shops
                      .Include(s => s.ItemCategories)
                      .FirstOrDefaultAsync(s => s.Id == id);
    }

}
