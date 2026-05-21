using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Shared.Exceptions;
using UnitTests.Application.Shared;

namespace UnitTests.Application.MovieEvents;

public sealed class CloseBookingTests : ApplicationTestBase
{
    private readonly BookMovieEvent _bookMovieEvent;
    private readonly CloseBooking _closeBooking;

    public CloseBookingTests()
    {
        _bookMovieEvent = new BookMovieEvent(UnitOfWork, CreateLogger<BookMovieEvent>());
        _closeBooking = new CloseBooking(UnitOfWork, CreateLogger<CloseBooking>());
    }

    [Fact]
    public async Task Execute_WithValidInputPaymentFailed_ShouldCloseBooking()
    {
        // Arrange
        Room room = Room.Create(
            "Test Room",
            100
        );
        await RoomRepository.Save(room);
        Movie movie = Movie.Create(
            "Movie title",
            "Movie description",
            ReleaseYear.Create(2025),
            Duration.Create(100),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure"), Genre.Create("Sci-Fi") },
            new List<Actor> { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt"), Actor.Create("Ellen Page") },
            AgeRating.Create(12),
            PosterUrl.Create("PosterUrl")
        );
        await MovieRepository.Save(movie);
        MovieEvent movieEvent = MovieEvent.Create(
            movie.Id,
            room.Id,
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);

        BookMovieEventInput bookMovieEventInput = new(
            movieEvent.Id.Value,
            2,
            1
        );

        Guid bookingId = await _bookMovieEvent.Execute(bookMovieEventInput);

        CloseBookingInput closeBookingInput = new(
            bookingId,
            "PaymentFailed"
        );

        // Act
        await _closeBooking.Execute(closeBookingInput);

        // Assert
        Booking booking = movieEvent.Bookings.First();
        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Failed, booking.PaymentStatus);
    }

    [Fact]
    public async Task Execute_WithValidInputPaymentSuccess_ShouldCloseBooking()
    {
        // Arrange
        Room room = Room.Create(
            "Test Room",
            100
        );
        await RoomRepository.Save(room);
        Movie movie = Movie.Create(
            "Movie title",
            "Movie description",
            ReleaseYear.Create(2025),
            Duration.Create(100),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure"), Genre.Create("Sci-Fi") },
            new List<Actor> { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt"), Actor.Create("Ellen Page") },
            AgeRating.Create(12),
            PosterUrl.Create("PosterUrl")
        );
        await MovieRepository.Save(movie);
        MovieEvent movieEvent = MovieEvent.Create(
            movie.Id,
            room.Id,
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);

        BookMovieEventInput bookMovieEventInput = new(
            movieEvent.Id.Value,
            2,
            1
        );

        Guid bookingId = await _bookMovieEvent.Execute(bookMovieEventInput);

        CloseBookingInput closeBookingInput = new(
            bookingId,
            "PaymentSuccess"
        );

        // Act
        await _closeBooking.Execute(closeBookingInput);

        // Assert
        Booking booking = movieEvent.Bookings.First();
        Assert.Equal(BookingStatus.Closed, booking.BookingStatus);
        Assert.Equal(PaymentStatus.Success, booking.PaymentStatus);
    }

    [Fact]
    public async Task Execute_WithInvalidBookingId_ShouldThrow()
    {
        // Arrange
        Room room = Room.Create(
            "Test Room",
            100
        );
        await RoomRepository.Save(room);
        Movie movie = Movie.Create(
            "Movie title",
            "Movie description",
            ReleaseYear.Create(2025),
            Duration.Create(100),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure"), Genre.Create("Sci-Fi") },
            new List<Actor> { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt"), Actor.Create("Ellen Page") },
            AgeRating.Create(12),
            PosterUrl.Create("PosterUrl")
        );
        await MovieRepository.Save(movie);
        MovieEvent movieEvent = MovieEvent.Create(
            movie.Id,
            room.Id,
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);

        BookMovieEventInput bookMovieEventInput = new(
            movieEvent.Id.Value,
            2,
            1
        );

        Guid bookingId = await _bookMovieEvent.Execute(bookMovieEventInput);

        CloseBookingInput closeBookingInput = new(
            Guid.NewGuid(), 
            "PaymentFailed"
        );

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _closeBooking.Execute(closeBookingInput));        
    }

    [Fact]
    public async Task Execute_WithInvalidReason_ShouldThrow()
    {
        // Arrange
        Room room = Room.Create(
            "Test Room",
            100
        );
        await RoomRepository.Save(room);
        Movie movie = Movie.Create(
            "Movie title",
            "Movie description",
            ReleaseYear.Create(2025),
            Duration.Create(100),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure"), Genre.Create("Sci-Fi") },
            new List<Actor> { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt"), Actor.Create("Ellen Page") },
            AgeRating.Create(12),
            PosterUrl.Create("PosterUrl")
        );
        await MovieRepository.Save(movie);
        MovieEvent movieEvent = MovieEvent.Create(
            movie.Id,
            room.Id,
            DateTime.UtcNow.AddDays(1).Date.AddHours(15),
            100
        );
        await MovieEventRepository.Save(movieEvent);

        BookMovieEventInput bookMovieEventInput = new(
            movieEvent.Id.Value,
            2,
            1
        );

        Guid bookingId = await _bookMovieEvent.Execute(bookMovieEventInput);

        CloseBookingInput closeBookingInput = new(
            bookingId,
            "InvalidReason"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _closeBooking.Execute(closeBookingInput));
    }
}