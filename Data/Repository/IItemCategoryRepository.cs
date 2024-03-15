using tongDe.Models;

namespace tongDe.Data.Repository;

public interface IItemCategoryRepository : IRepository<ItemCategory>
{
    Task<IEnumerable<ItemCategory>> GetItemCategoriesAsync(int shopId);
}
