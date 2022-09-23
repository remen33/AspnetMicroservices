using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger) 
        {
            this.mapper = mapper;
            this.orderRepository = orderRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDeleted = await orderRepository.GetByIdAsync(request.Id);

            if (orderToDeleted == null)
                logger.LogError("Order mpt exist on database.");

            await orderRepository.DeleteAsync(orderToDeleted);
        }
    }
}
