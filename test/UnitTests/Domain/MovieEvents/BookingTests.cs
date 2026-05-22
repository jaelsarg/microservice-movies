using Howestprime.Movies.Domain.MovieEvents;

namespace UnitTests.Domain.MovieEvents;

public sealed class BookingTests
{
    [Fact]
    public void Create_WithValidInput_ShouldCreateBooking()
    {
        // Arrange
        BookingStatus bookingStatus = BookingStatus.Open;
        PaymentStatus paymentStatus = PaymentStatus.Pending;
        int standardVisitors = 2;
        int discountVisitors = 1;
        List<int> seatNumbers = new() { 1, 2, 3 };

        // Act
        Booking booking = Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountVisitors, seatNumbers);

        // Assert
        Assert.Equal(bookingStatus, booking.BookingStatus);
        Assert.Equal(paymentStatus, booking.PaymentStatus);
        Assert.Equal(standardVisitors, booking.StandardVisitors);
        Assert.Equal(discountVisitors, booking.DiscountVisitors);
        Assert.Equal(seatNumbers, booking.SeatNumbers);
    }

    [Fact]
    public void Create_WithInvalidVisitors_ShouldThrowArgumentException()
    {
        // Arrange
        BookingStatus bookingStatus = BookingStatus.Open;
        PaymentStatus paymentStatus = PaymentStatus.Pending;
        int standardVisitors = 0;
        int discountVisitors = 0;
        List<int> seatNumbers = new();

        // Act
        Action act = () => Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountVisitors, seatNumbers);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithInvalidSeatNumbers_ShouldThrowArgumentException()
    {
        // Arrange
        BookingStatus bookingStatus = BookingStatus.Open;
        PaymentStatus paymentStatus = PaymentStatus.Pending;
        int standardVisitors = 2;
        int discountVisitors = 1;
        List<int> seatNumbers = new() { 1, 2 };

        // Act
        Action act = () => Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountVisitors, seatNumbers);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}