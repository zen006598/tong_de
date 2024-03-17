using tongDe.Models;

namespace tongDe.Data.Repository.Interfaces;
public interface IShopRepository : IRepository<Shop>
{
    Task<IEnumerable<Shop>> GetShops(string userId);
    Task CancelAsync(int id);

    Task<Shop> GetShopWithItemCategoriesAsync(int id);
}
