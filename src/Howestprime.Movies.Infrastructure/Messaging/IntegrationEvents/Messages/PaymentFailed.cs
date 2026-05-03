namespace Howestprime.Movies.Infrastructure.Messaging.IntegrationEvents.Messages;

public sealed record PaymentFailed(
    Guid BookingId
);