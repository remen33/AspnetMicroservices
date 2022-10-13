using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Respositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {   
        public OrderRepository(OrderContext dbContext): base(dbContext)
        {
            
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await dbContext.Orders
                 .Where(o => o.UserName == userName)
                 .ToListAsync();            
        }
    }
}
