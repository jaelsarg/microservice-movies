namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;

public sealed record PaymentSuccess(
    Guid BookingId
);