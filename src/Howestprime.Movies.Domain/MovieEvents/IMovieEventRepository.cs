using Aornis;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.MovieEvents;

public interface IMovieEventRepository : IRepository<MovieEvent, MovieEventId>
{
    Task<IEnumerable<MovieEvent>> ByShowtimeAndRoomID(DateTime showtime, RoomId roomId);
    Task<Optional<MovieEvent>> ByBookingId(BookingId bookingId);
}