using tongDe.Models;

namespace tongDe.Data.Repository;

public class ItemAliasRepository : Repository<ItemAlias>, IItemAliasRepository
{
    public ItemAliasRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
