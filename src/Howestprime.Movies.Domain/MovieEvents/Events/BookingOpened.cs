using Howestprime.Movies.Domain.Movies;

namespace Howestprime.Movies.Domain.MovieEvents.Events;

public sealed class BookingOpened(
    BookingId bookingId,
    MovieId movieId,
    string room,
    DateTime showtime,
    int standardVisitors,
    int discountVisitors,
    List<int> SeatNumbers
) : MovieEventDomainEvent(nameof(BookingOpened))
{
    public BookingId BookingId { get; private init; } = bookingId;
    public MovieId MovieId { get; private init; } = movieId;
    public string Room { get; private init; } = room;
    public DateTime Showtime { get; private init; } = showtime;
    public int StandardVisitors { get; private init; } = standardVisitors;
    public int DiscountVisitors { get; private init; } = discountVisitors;
    public List<int> SeatNumbers { get; private init; } = SeatNumbers;
}