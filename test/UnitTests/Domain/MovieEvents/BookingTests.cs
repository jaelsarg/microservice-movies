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
        int discountedVisitors = 1;
        List<int> seatNumbers = new() { 1, 2, 3 };

        // Act
        Booking booking = Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountedVisitors, seatNumbers);

        // Assert
        Assert.Equal(bookingStatus, booking.BookingStatus);
        Assert.Equal(paymentStatus, booking.PaymentStatus);
        Assert.Equal(standardVisitors, booking.StandardVisitors);
        Assert.Equal(discountedVisitors, booking.DiscountedVisitors);
        Assert.Equal(seatNumbers, booking.SeatNumbers);
    }

    [Fact]
    public void Create_WithInvalidVisitors_ShouldThrowArgumentException()
    {
        // Arrange
        BookingStatus bookingStatus = BookingStatus.Open;
        PaymentStatus paymentStatus = PaymentStatus.Pending;
        int standardVisitors = 0;
        int discountedVisitors = 0;
        List<int> seatNumbers = new();

        // Act
        Action act = () => Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountedVisitors, seatNumbers);

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
        int discountedVisitors = 1;
        List<int> seatNumbers = new() { 1, 2 };

        // Act
        Action act = () => Booking.Create(bookingStatus, paymentStatus, standardVisitors, discountedVisitors, seatNumbers);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}