using Aornis;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Rooms;

namespace UnitTests.Application.Mocks;

public sealed class InMemoryMovieEventRepository : IMovieEventRepository
{
    private readonly HashSet<MovieEvent> _movieEvents = new();

    public Task<Optional<MovieEvent>> ById(MovieEventId id)
    {
        return Task.FromResult(
            Optional.Of(_movieEvents.FirstOrDefault(movieEvent => movieEvent.Id.Equals(id)))
        );
    }

    public Task<IEnumerable<MovieEvent>> ByShowtimeAndRoomID(DateTime showtime, RoomId roomId)
    {
        return Task.FromResult(
            _movieEvents.Where(movieEvent => movieEvent.Showtime == showtime && movieEvent.RoomId.Equals(roomId))
        );
    }

    public Task<Optional<MovieEvent>> ByBookingId(BookingId bookingId)
    {
        return Task.FromResult(
            Optional.Of(_movieEvents.FirstOrDefault(movieEvent => movieEvent.Bookings.Any(booking => booking.Id.Equals(bookingId))))
        );
    }

    public Task<bool> Exists(MovieEventId id)
    {
        return Task.FromResult(
            _movieEvents.Any(movie => movie.Id.Equals(id))
        );
    }

    public Task Remove(MovieEvent aggregateRoot)
    {
        return Task.FromResult(
            _movieEvents.Remove(aggregateRoot)
        );
    }

    public Task Save(MovieEvent aggregateRoot)
    {
        _movieEvents.Add(aggregateRoot);
        return Task.CompletedTask;
    }
}