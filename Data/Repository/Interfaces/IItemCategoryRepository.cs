using tongDe.Models;

namespace tongDe.Data.Repository.Interfaces;

public interface IItemCategoryRepository : IRepository<ItemCategory>
{
    Task<IEnumerable<ItemCategory>> GetItemCategoriesAsync(int shopId);
    Task<ItemCategory> GetItemCategoryWithItemsAsync(int id);
}
