using Howestprime.Movies.Domain.MovieEvents.Events;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.MovieEvents;

public readonly record struct MovieEventId(Guid Value) : IEntityId;

public sealed class MovieEvent : AggregateRoot<MovieEventId>
{
    public MovieId MovieId { get; private set; }
    public RoomId RoomId { get; private set; }
    public DateTime Showtime { get; private set; }
    public int Capacity { get; private set; }
    public List<Booking> Bookings { get; private set; } = new List<Booking>();
    public int Visitors { get; private set; }

    private MovieEvent(
        MovieEventId id,
        MovieId movieId,
        RoomId roomId,
        DateTime showtime,
        int capacity,
        List<Booking> bookings,
        int visitors
    ) : base(id)
    {
        MovieId = movieId;
        RoomId = roomId;
        Showtime = showtime;
        Capacity = capacity;
        Bookings = bookings;
        Visitors = visitors;
    }
    
    #pragma warning disable CS8618
    private MovieEvent() :base(default!){}
    #pragma warning restore CS8618

    public static MovieEvent Create(
        MovieId movieId,
        RoomId roomId,
        DateTime showtime,
        int capacity
    )
    {
        MovieEvent movieEvent = new(
            EntityId.New<MovieEventId>(),
            movieId,
            roomId,
            showtime,
            capacity,
            new List<Booking>(),
            0
        );

        movieEvent.ValidateState();

        return movieEvent;
    }

    public override void ValidateState()
    {
        Asserts.EnsureNotEmpty(MovieId);
        Asserts.EnsureNotEmpty(RoomId);
        Asserts.EnsureNotNegative(Capacity);
        MovieEventAsserts.EnsureShowTimeIsAt15hOr19h(Showtime);
        MovieEventAsserts.EnsureShowtimeIsInTheFuture(Showtime);
    }

    public void UpdateMovieId(MovieId movieId)
    {
        MovieId = movieId;
    }

    public BookingId Book(int standardVisitors, int discountVisitors, string roomName)
    {
        int totalVisitors = standardVisitors + discountVisitors;
        List<string> seatNumbers = new();

        for (int i = 0; i < totalVisitors; i++)
        {
            seatNumbers.Add((Visitors + i + 1).ToString());
        }

        Booking booking = Booking.Create(
            BookingStatus.Open,
            PaymentStatus.Pending,
            standardVisitors,
            discountVisitors,
            seatNumbers
        );

        Bookings.Add(booking);
        Visitors += totalVisitors;

        RaiseDomainEvent(new BookingOpened(
            booking.Id,
            MovieId,
            roomName,
            Showtime,
            standardVisitors,
            discountVisitors,
            seatNumbers
        ));

        return booking.Id;
    }

    public void CloseBooking(BookingId bookingId, CloseBookingReason reason)
    {
        Booking booking = Bookings.FirstOrDefault(b => b.Id == bookingId) ?? throw new InvalidOperationException($"Booking with id {bookingId} not found.");

        if (reason == CloseBookingReason.PaymentSuccess)
        {
            booking.Close();
            booking.MarkPaymentAsPaid();
        }
        else
        {
            booking.Close();
            booking.MarkPaymentAsFailed();
            booking.ReleaseSeats();
            Visitors -= booking.StandardVisitors + booking.DiscountVisitors;
        }
    }
}