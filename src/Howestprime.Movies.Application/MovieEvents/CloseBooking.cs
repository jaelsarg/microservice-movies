using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.MovieEvents;

public sealed record CloseBookingInput(
    Guid BookingId,
    CloseBookingReason Reason
);

public sealed class CloseBooking(
    IUnitOfWork uow,
    ILogger<CloseBooking> logger
) : IUseCase<CloseBookingInput>
{
    public async Task Execute(CloseBookingInput input)
    {
        logger.LogDebug($"Executing {nameof(CloseBooking)} for booking with id {input.BookingId} and reason {input.Reason}.");

        BookingId bookingId = new(input.BookingId);
        Optional<MovieEvent> movieEventOptional = await uow.Repo<IMovieEventRepository>().ByBookingId(bookingId);
        if (!movieEventOptional.HasValue) throw new NotFoundException($"Booking with id {input.BookingId} not found.");
        MovieEvent movieEvent = movieEventOptional.Value;

        CloseBookingReason reason = input.Reason;

        movieEvent.CloseBooking(bookingId, reason);

        await uow.Save<IMovieEventRepository>(movieEvent);

        await uow.Do();

        logger.LogInformation($"Closed booking with id {input.BookingId} for reason: {input.Reason}.");
    }
}