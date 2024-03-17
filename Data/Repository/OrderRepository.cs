using tongDe.Data.Repository.Interfaces;
using tongDe.Models;

namespace tongDe.Data.Repository;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
