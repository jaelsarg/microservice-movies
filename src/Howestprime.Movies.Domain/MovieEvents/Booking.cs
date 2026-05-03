using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.MovieEvents;

public readonly record struct BookingId(Guid Value) : IEntityId;

public sealed class Booking : Entity<BookingId>
{
    public BookingStatus BookingStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public int StandardVisitors { get; private set; }
    public int DiscountVisitors { get; private set; }
    public List<string> SeatNumbers { get; private set; }

    private Booking(
        BookingId id,
        BookingStatus bookingStatus,
        PaymentStatus paymentStatus,
        int standardVisitors,
        int discountVisitors,
        List<string> seatNumbers
    ) : base(id)
    {
        BookingStatus = bookingStatus;
        PaymentStatus = paymentStatus;
        StandardVisitors = standardVisitors;
        DiscountVisitors = discountVisitors;
        SeatNumbers = seatNumbers;
    }

    #pragma warning disable CS8618
    private Booking() : base(default!) {}
    #pragma warning restore CS8618

    public static Booking Create(
        BookingStatus bookingStatus,
        PaymentStatus paymentStatus,
        int standardVisitors,
        int discountVisitors,
        List<string> seatNumbers
    )
    {
        Booking booking = new(
            EntityId.New<BookingId>(),
            bookingStatus,
            paymentStatus,
            standardVisitors,
            discountVisitors,
            seatNumbers
        );

        booking.ValidateState();

        return booking;
    }

    public override void ValidateState()
    {
        if (StandardVisitors + DiscountVisitors <= 0) throw new ArgumentException("The total number of visitors must be greater than 0.");
        if (SeatNumbers.Count != StandardVisitors + DiscountVisitors) throw new ArgumentException("The number of seat numbers must match the total number of visitors.");
    }

    public void Close()
    {
        BookingStatus = BookingStatus.Closed;
    }

    public void MarkPaymentAsPaid()
    {
        PaymentStatus = PaymentStatus.Success;
    }

    public void MarkPaymentAsFailed()
    {
        PaymentStatus = PaymentStatus.Failed;
    }

    public void ReleaseSeats()
    {
        SeatNumbers.Clear();
    }
}