using Howestprime.Movies.Application.Contracts.Data;

namespace UnitTests.Application.Contracts.Data;

public sealed class MovieEventDataTests
{
    [Fact]
    public void Create_WithValues_ShouldSetProperties()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        DateTime showtime = new DateTime(2025, 12, 25, 20, 0, 0);
        int capacity = 100;
        RoomData room = new RoomData
        {
            Id = Guid.NewGuid(),
            Name = "Room 1",
            Capacity = 150
        };
        MovieData movie = new MovieData
        {
            Id = Guid.NewGuid(),
            PosterUrl = "https://example.com/poster.jpg",
            Title = "Inception",
            Genres = new List<GenreData>
            {
                new GenreData { Value = "Sci-Fi" },
                new GenreData { Value = "Action" }
            },
            Actors = new List<ActorData>
            {
                new ActorData { Value = "Leonardo DiCaprio" },
                new ActorData { Value = "Joseph Gordon-Levitt" }
            },
            AgeRating = 13,
            ReleaseYear = 2010,
            Duration = 148,
            Description = "A skilled thief uses dream-sharing technology to plant an idea into the mind of a CEO."
        };

        // Act
        MovieEventData movieEventData = new MovieEventData
        {
            Id = id,
            Showtime = showtime,
            Capacity = capacity,
            Room = room,
            Movie = movie
        };        

        // Assert
        Assert.Equal(id, movieEventData.Id);
        Assert.Equal(showtime, movieEventData.Showtime);
        Assert.Equal(capacity, movieEventData.Capacity);
        Assert.Equal(room, movieEventData.Room);
        Assert.Equal(movie, movieEventData.Movie);
    }

    [Fact]
    public void Create_WithDefaults_ShouldInitializeValues()
    {
        // Arrange & Act
        MovieEventData movieEventData = new MovieEventData();

        // Assert
        Assert.Equal(Guid.Empty, movieEventData.Id);
        Assert.Equal(DateTime.MinValue, movieEventData.Showtime);
        Assert.Equal(0, movieEventData.Capacity);
        Assert.Null(movieEventData.Room);
        Assert.Null(movieEventData.Movie);
    }
}