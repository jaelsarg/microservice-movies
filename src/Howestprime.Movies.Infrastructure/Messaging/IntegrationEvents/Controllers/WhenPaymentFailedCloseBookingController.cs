using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Contracts;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Controllers;

public sealed class WhenPaymentFailedCloseBookingController(
    IUseCase<CloseBookingInput> useCase,
    ILogger<WhenPaymentFailedCloseBookingController> logger
) : IController<ConsumerContext>
{
    public async Task Handle(ConsumerContext context)
    {
        logger.LogInformation($"Invoking use case {nameof(CloseBooking)} for message {context.Message}.");

        PaymentFailed body = AmqpMessageConverter.ParseBody<PaymentFailed>(context);

        CloseBookingInput input = new(
            BookingId: body.BookingId,
            Reason: "PaymentFailed"
        );

        await useCase.Execute(input);
    }
}