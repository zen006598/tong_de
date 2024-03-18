using tongDe.Models;

namespace tongDe.Data.Repository.Interfaces;
public interface IItemRepository : IRepository<Item>
{
    Task<IEnumerable<Item>> GetItems(int shopId);
    Task<Item> GetItemWithAliasesAsync(int id);
}
