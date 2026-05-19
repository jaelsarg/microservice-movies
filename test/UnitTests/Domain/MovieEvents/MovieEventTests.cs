using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.MovieEvents.Events;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;

namespace UnitTests.Domain.MovieEvents;

public sealed class MovieEventTests
{
    [Fact]
    public void Create_WithValidInput_ShouldCreateMovieEvent()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(1).Date.AddHours(15);
        int capacity = 100;

        // Act
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, showtime, capacity);

        // Assert
        Assert.Equal(movieId, movieEvent.MovieId);
        Assert.Equal(roomId, movieEvent.RoomId);
        Assert.Equal(showtime, movieEvent.Showtime);
        Assert.Equal(capacity, movieEvent.Capacity);
    }

    [Fact]
    public void Create_WithInvalidCapacity_ShouldThrowArgumentException()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(1).Date.AddHours(15);
        int capacity = -1;
    
        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, showtime, capacity);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithInvalidTime_ShouldThrowArgumentException()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(1).Date.AddHours(14);
        int capacity = 100;
    
        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, showtime, capacity);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithInvalidDate_ShouldThrowArgumentException()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(-1).Date.AddHours(15);
        int capacity = 100;
    
        // Act
        Action act = () => MovieEvent.Create(movieId, roomId, showtime, capacity);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void UpdateMovieId_ShouldUpdateMovieId()
    {
        // Arrange
        MovieId originalMovieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(1).Date.AddHours(15);
        int capacity = 100;
        MovieEvent movieEvent = MovieEvent.Create(originalMovieId, roomId, showtime, capacity);
        MovieId newMovieId = EntityId.New<MovieId>();

        // Act
        movieEvent.UpdateMovieId(newMovieId);

        // Assert
        Assert.Equal(newMovieId, movieEvent.MovieId);
    }

    [Fact]
    public void Book_WithValidInput_ShouldBookMovieEventAndRaiseEvent()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        RoomId roomId = EntityId.New<RoomId>();
        DateTime showtime = DateTime.Now.AddDays(1).Date.AddHours(15);
        int capacity = 100;
        MovieEvent movieEvent = MovieEvent.Create(movieId, roomId, showtime, capacity);

        int standardVisitors = 2;
        int discountedVisitors = 1;
        string roomName = "Room 1";

        // Act
        BookingId bookingId = movieEvent.Book(standardVisitors, discountedVisitors, roomName);

        // Assert
        Assert.Equal(standardVisitors + discountedVisitors, movieEvent.Visitors);
        Assert.Single(movieEvent.Bookings);
        Assert.Contains(
            movieEvent.DomainEvents,
            e => e is BookingOpened opened &&
                opened.BookingId == bookingId &&
                opened.MovieId == movieId &&
                opened.Room == roomName &&
                opened.Showtime == showtime &&
                opened.StandardVisitors == standardVisitors &&
                opened.DiscountedVisitors == discountedVisitors
        );
    }
}