using tongDe.Data.Repository.Interfaces;
using tongDe.Models;

namespace tongDe.Data.Repository;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
