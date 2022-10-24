using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<BasketCheckoutConsumer> logger;
        private readonly IMediator mediator;

        public BasketCheckoutConsumer(IMapper mapper,
            ILogger<BasketCheckoutConsumer> logger, IMediator mediator)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await mediator.Send(command);

            logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}
