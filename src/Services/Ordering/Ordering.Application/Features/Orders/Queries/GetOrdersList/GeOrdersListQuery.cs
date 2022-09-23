using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GeOrdersListQuery :IRequest<List<OrdersVm>>
    {
        public readonly string userName;

        public GeOrdersListQuery(string userName)
        {            
            this.userName = userName ?? throw new ArgumentNullException(userName);
        }
    }
}
