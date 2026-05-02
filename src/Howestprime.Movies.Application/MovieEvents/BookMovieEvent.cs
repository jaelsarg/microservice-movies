using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.MovieEvents;

public sealed record BookMovieEventInput(
    Guid MovieEventId,
    int StandardVisitors,
    int DiscountVisitors
);

public sealed class BookMovieEvent(
    IUnitOfWork uow,
    ILogger<BookMovieEvent> logger
) : IUseCase<BookMovieEventInput, Guid>
{
    public async Task<Guid> Execute(BookMovieEventInput input)
    {
        if (input.StandardVisitors + input.DiscountVisitors <= 0) throw new ArgumentException("The total number of visitors must be greater than 0.");

        MovieEventId movieEventId = new(input.MovieEventId);
        Optional<MovieEvent> movieEventOptional = await uow.Repo<IMovieEventRepository>().ById(movieEventId);
        if (!movieEventOptional.HasValue) throw new NotFoundException($"Movie event with id {input.MovieEventId} not found.");
        MovieEvent movieEvent = movieEventOptional.Value;

        if (movieEvent.Visitors + input.StandardVisitors + input.DiscountVisitors > movieEvent.Capacity) throw new InvalidOperationException("Not enough capacity for the number of visitors.");
        if (movieEvent.Showtime > DateTime.UtcNow.AddDays(14)) throw new InvalidOperationException("Bookings can only be made within 14 days of the showtime.");

        Optional<Room> roomOptional = await uow.Repo<IRoomRepository>().ById(movieEvent.RoomId);
        if (!roomOptional.HasValue) throw new NotFoundException($"Room with id {movieEvent.RoomId.Value} not found.");
        Room room = roomOptional.Value;

        BookingId bookingId = movieEvent.Book(input.StandardVisitors, input.DiscountVisitors, room.Name);

        await uow.Repo<IMovieEventRepository>().Save(movieEvent);

        await uow.Do();

        logger.LogInformation($"Booked booking with id {bookingId.Value} for movie event {input.MovieEventId}.");

        return bookingId.Value;
    }
}