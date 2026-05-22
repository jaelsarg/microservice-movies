using Aornis;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;
using UnitTests.Application.Shared;

namespace UnitTests.Application.MovieEvents;

public sealed class BookMovieEventTests : ApplicationTestBase
{
    private readonly BookMovieEvent _bookMovieEvent;

    public BookMovieEventTests()
    {
        _bookMovieEvent = new BookMovieEvent(UnitOfWork, CreateLogger<BookMovieEvent>());
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldBookMovieEvent()
    {
        // Arrange
        MovieEvent movieEvent = MovieEvent.Create(
            EntityId.New<MovieId>(),
            EntityId.New<RoomId>(),
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);
        Room room = Room.Create("Test Room", 100, movieEvent.RoomId);
        await RoomRepository.Save(room);
        BookMovieEventInput input = new(
            movieEvent.Id.Value,
            2,
            1
        );

        // Act
        Guid bookingId = await _bookMovieEvent.Execute(input);

        // Assert
        Optional<MovieEvent> movieEventOptional = await MovieEventRepository.ById(movieEvent.Id);
        Assert.True(movieEventOptional.HasValue);
        MovieEvent updatedMovieEvent = movieEventOptional.Value;
        Assert.Equal(movieEvent.Id, updatedMovieEvent.Id);
        Assert.Equal(movieEvent.MovieId, updatedMovieEvent.MovieId);
        Assert.Equal(movieEvent.RoomId, updatedMovieEvent.RoomId);
        Assert.Equal(movieEvent.Showtime, updatedMovieEvent.Showtime);
        Assert.Equal(movieEvent.Capacity, updatedMovieEvent.Capacity);
        Assert.Equal(3, updatedMovieEvent.Visitors);
        Assert.Single(updatedMovieEvent.Bookings);
        Booking booking = updatedMovieEvent.Bookings.First();
        Assert.Equal(bookingId, booking.Id.Value);
        Assert.Equal(BookingStatus.Open, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Pending, booking.PaymentStatus);
        Assert.Equal(2, booking.StandardVisitors);
        Assert.Equal(1, booking.DiscountVisitors);
        Assert.Equal(new List<int> { 1, 2, 3 }, booking.SeatNumbers);
    }

    [Fact]
    public async Task Execute_WithInvalidVisitors_ShouldThrow()
    {
        // Arrange
        MovieEvent movieEvent = MovieEvent.Create(
            EntityId.New<MovieId>(),
            EntityId.New<RoomId>(),
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);
        BookMovieEventInput input = new(
            movieEvent.Id.Value,
            0,
            0
        );

        // Act
        Task act = _bookMovieEvent.Execute(input);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => act);
    }
}