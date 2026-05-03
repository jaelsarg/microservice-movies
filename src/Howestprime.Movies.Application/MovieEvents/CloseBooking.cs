using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.MovieEvents;

public sealed record CloseBookingInput(
    Guid BookingId,
    string Reason
);

public sealed class CloseBooking(
    IUnitOfWork uow,
    ILogger<CloseBooking> logger
) : IUseCase<CloseBookingInput>
{
    public async Task Execute(CloseBookingInput input)
    {
        BookingId bookingId = new(input.BookingId);
        Optional<MovieEvent> movieEventOptional = await uow.Repo<IMovieEventRepository>().ByBookingId(bookingId);
        if (!movieEventOptional.HasValue) throw new NotFoundException($"Booking with id {input.BookingId} not found.");
        MovieEvent movieEvent = movieEventOptional.Value;

        if (input.Reason != "PaymentFailed" && input.Reason != "PaymentSuccess") throw new ArgumentException($"Invalid reason for closing booking: {input.Reason}.");

        CloseBookingReason reason = input.Reason == "PaymentFailed"
            ? CloseBookingReason.PaymentFailed
            : CloseBookingReason.PaymentSuccess;

        movieEvent.CloseBooking(bookingId, reason);

        await uow.Save<IMovieEventRepository>(movieEvent);

        await uow.Do();

        logger.LogInformation($"Closed booking with id {input.BookingId} for reason: {input.Reason}.");
    }
}