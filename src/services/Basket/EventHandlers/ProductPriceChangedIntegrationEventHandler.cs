using Basket.Services;
using MassTransit;
using ServiceDefaults.Messaging.Events;

namespace Basket.EventHandlers
{
    public class ProductPriceChangedIntegrationEventHandler(BasketService basketService)
    : IConsumer<ProductPriceChangedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
        {
            await basketService.
                UpdateBasketPrices(context.Message.ProductId , context.Message.Price);
        }
    }
}
