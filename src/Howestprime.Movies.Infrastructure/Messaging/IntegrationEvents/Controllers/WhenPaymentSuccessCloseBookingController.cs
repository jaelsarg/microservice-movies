using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Contracts;
using Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Controllers;

public sealed class WhenPaymentSuccessCloseBookingController(
    IUseCase<CloseBookingInput> useCase,
    ILogger<WhenPaymentSuccessCloseBookingController> logger
) : IController<ConsumerContext>
{
    public async Task Handle(ConsumerContext context)
    {
        logger.LogInformation($"Invoking use case {nameof(CloseBooking)} for message {context.Message}.");

        PaymentSuccess body = AmqpMessageConverter.ParseBody<PaymentSuccess>(context);

        CloseBookingInput input = new(
            BookingId: body.BookingId,
            Reason: "PaymentSuccess"
        );

        await useCase.Execute(input);
    }
}