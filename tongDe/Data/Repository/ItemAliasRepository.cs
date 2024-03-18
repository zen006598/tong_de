using tongDe.Models;
using tongDe.Data.Repository.Interfaces;

namespace tongDe.Data.Repository;

public class ItemAliasRepository : Repository<ItemAlias>, IItemAliasRepository
{
    public ItemAliasRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
