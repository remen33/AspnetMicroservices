using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GeOrdersListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<OrdersVm>> Handle(GeOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await orderRepository.GetOrdersByUserName(request.userName);
            return mapper.Map<List<OrdersVm>>(orderList);
        }
    }
}
