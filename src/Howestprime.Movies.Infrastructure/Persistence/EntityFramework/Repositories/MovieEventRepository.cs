using Aornis;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class MovieEventRepository(
    DomainDbContext context
) : EfCoreGenericRepository<MovieEvent, MovieEventId>(context), IMovieEventRepository
{
    public Task<IEnumerable<MovieEvent>> ByShowtimeAndRoomID(DateTime showtime, RoomId roomId)
    {
        IEnumerable<MovieEvent> movieEvents = _context.MovieEvents
            .Where(movieEvent => movieEvent.Showtime == showtime && movieEvent.RoomId == roomId);
        
        return Task.FromResult(movieEvents);
    }

    public Task<Optional<MovieEvent>> ByBookingId(BookingId bookingId)
    {
        Optional<MovieEvent> movieEvent = _context.MovieEvents
            .Where(movieEvent => movieEvent.Bookings.Any(booking => booking.Id == bookingId))
            .FirstOrDefault();

        return Task.FromResult(movieEvent);
    }
}