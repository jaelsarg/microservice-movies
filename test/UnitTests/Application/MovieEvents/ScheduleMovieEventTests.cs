using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Shared.Exceptions;
using UnitTests.Application.Shared;

namespace UnitTests.Application.MovieEvents;

public sealed class ScheduleMovieEventTests : ApplicationTestBase
{
    private readonly ScheduleMovieEvent _scheduleMovieEvent;
    private readonly Guid _movieId = Guid.NewGuid();
    private readonly Guid _roomId = Guid.NewGuid();

    public ScheduleMovieEventTests()
    {
        _scheduleMovieEvent = new ScheduleMovieEvent(UnitOfWork, CreateLogger<ScheduleMovieEvent>());
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldScheduleMovieEvent()
    {
        // Arrange
        DateTime showtime = new DateTime(2026, 8, 12, 19, 0, 0);

        Movie movie = Movie.Create(
            "Test Movie",
            "A test movie description.",
            ReleaseYear.Create(2025),
            Duration.Create(120),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure") },
            new List<Actor> { Actor.Create("Actor 1"), Actor.Create("Actor 2") },
            AgeRating.Create(16),
            PosterUrl.Create("http://example.com/poster.jpg"),
            EntityId.New<MovieId>(_movieId)
        );

        Room room = Room.Create(
            "Room 1",
            100,
            EntityId.New<RoomId>(_roomId)
        );

        await MovieRepository.Save(movie);
        await RoomRepository.Save(room);

        // Act
        Guid movieEventId = await _scheduleMovieEvent.Execute(new ScheduleMovieEventInput(_movieId, showtime, _roomId));

        // Assert
        MovieEvent createdMovieEvent = (await MovieEventRepository.ById(new MovieEventId(movieEventId))).Value;
        Assert.Equal(EntityId.New<MovieId>(_movieId), createdMovieEvent.MovieId);
        Assert.Equal(EntityId.New<RoomId>(_roomId), createdMovieEvent.RoomId);
        Assert.Equal(showtime, createdMovieEvent.Showtime);
    }

    [Fact]
    public async Task Execute_WithNonExistingMovie_ShouldThrowNotFoundException()
    {
        // Arrange
        Guid nonExistingMovieId = Guid.NewGuid();
        DateTime showtime = new DateTime(2025, 5, 12, 19, 0, 0);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _scheduleMovieEvent.Execute(new ScheduleMovieEventInput(nonExistingMovieId, showtime, _roomId)));
    }

    [Fact]
    public async Task Execute_WithNonExistingRoom_ShouldThrowNotFoundException()
    {
        // Arrange
        Guid nonExistingRoomId = Guid.NewGuid();
        DateTime showtime = new DateTime(2025, 5, 12, 19, 0, 0);

        Movie movie = Movie.Create(
            "Test Movie",
            "A test movie description.",
            ReleaseYear.Create(2025),
            Duration.Create(120),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure") },
            new List<Actor> { Actor.Create("Actor 1"), Actor.Create("Actor 2") },
            AgeRating.Create(16),
            PosterUrl.Create("http://example.com/poster.jpg"),
            EntityId.New<MovieId>(_movieId)
        );

        await MovieRepository.Save(movie);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _scheduleMovieEvent.Execute(new ScheduleMovieEventInput(_movieId, showtime, nonExistingRoomId)));
    }

    [Fact]
    public async Task Execute_WithExistingMovieEvent_ShouldUpdateMovieEvent()
    {
        // Arrange
        DateTime showtime = new DateTime(2026, 8, 12, 19, 0, 0);
        Guid movieId2 = Guid.NewGuid();

        Movie movie1 = Movie.Create(
            "Test Movie 1",
            "A test movie description.",
            ReleaseYear.Create(2025),
            Duration.Create(120),
            new List<Genre> { Genre.Create("Action"), Genre.Create("Adventure") },
            new List<Actor> { Actor.Create("Actor 1"), Actor.Create("Actor 2") },
            AgeRating.Create(16),
            PosterUrl.Create("http://example.com/poster1.jpg"),
            EntityId.New<MovieId>(_movieId)
        );

        Movie movie2 = Movie.Create(
            "Test Movie 2",
            "Another test movie description.",
            ReleaseYear.Create(2025),
            Duration.Create(90),
            new List<Genre> { Genre.Create("Comedy") },
            new List<Actor> { Actor.Create("Actor 3"), Actor.Create("Actor 4") },
            AgeRating.Create(12),
            PosterUrl.Create("http://example.com/poster2.jpg"),
            EntityId.New<MovieId>(movieId2)
        );

        Room room = Room.Create(
            "Room 1",
            100,
            EntityId.New<RoomId>(_roomId)
        );

        await MovieRepository.Save(movie1);
        await MovieRepository.Save(movie2);
        await RoomRepository.Save(room);

        MovieEvent existingMovieEvent = MovieEvent.Create(
            movie1.Id,
            room.Id,
            showtime,
            room.Capacity
        );

        await MovieEventRepository.Save(existingMovieEvent);

        // Act
        Guid movieEventId = await _scheduleMovieEvent.Execute(new ScheduleMovieEventInput(movieId2, showtime, _roomId));

        // Assert
        MovieEvent updatedMovieEvent = (await MovieEventRepository.ById(new MovieEventId(movieEventId))).Value;
        Assert.Equal(movie2.Id, updatedMovieEvent.MovieId);
    }
}