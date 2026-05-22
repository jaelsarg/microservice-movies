using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;

namespace UnitTests.Application.MovieEvents;

public sealed class SearchMovieEventsInTimerangeTests
{
    [Fact]
    public async Task Execute_WithInput_ShouldFilterMovieEvents()
    {
        // Arrange
        DateTime fromDate = new DateTime(2026, 5, 1);
        DateTime toDate = new DateTime(2026, 5, 31);

        List<MovieEventData> data =
        [
            new MovieEventData
            {
                Id = Guid.NewGuid(),
                Showtime = new DateTime(2026, 5, 10, 19, 0, 0),
                Capacity = 100,
                Room = new RoomData
                {
                    Id = Guid.NewGuid(),
                    Name = "Room 1",
                    Capacity = 100
                },
                Movie = new MovieData
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 1",
                    Description = "Description 1",
                    ReleaseYear = 2025,
                    Duration = 120,
                    Genres = new List<GenreData> { new GenreData { Value = "Action" }, new GenreData { Value = "Adventure" } },
                    Actors = new List<ActorData> { new ActorData { Value = "Actor 1" }, new ActorData { Value = "Actor 2" } },
                    AgeRating = 16,
                    PosterUrl = "http://example.com/poster1.jpg"
                }
            },
            new MovieEventData
            {
                Id = Guid.NewGuid(),
                Showtime = new DateTime(2026, 6, 10, 20, 0, 0),
                Capacity = 50,
                Room = new RoomData
                {
                    Id = Guid.NewGuid(),
                    Name = "Room 2",
                    Capacity = 50
                },
                Movie = new MovieData
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 2",
                    Description = "Description 2",
                    ReleaseYear = 2024,
                    Duration = 90,
                    Genres = new List<GenreData> { new GenreData { Value = "Comedy" } },
                    Actors = new List<ActorData> { new ActorData { Value = "Actor 3" }, new ActorData { Value = "Actor 4" } },
                    AgeRating = 12,
                    PosterUrl = "http://example.com/poster2.jpg"
                }
            },
            new MovieEventData
            {
                Id = Guid.NewGuid(),
                Showtime = new DateTime(2026, 5, 20, 18, 0, 0),
                Capacity = 75,
                Room = new RoomData
                {
                    Id = Guid.NewGuid(),
                    Name = "Room 3",
                    Capacity = 75
                },
                Movie = new MovieData
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie 3",
                    Description = "Description 3",
                    ReleaseYear = 2023,
                    Duration = 110,
                    Genres = new List<GenreData> { new GenreData { Value = "Drama" } },
                    Actors = new List<ActorData> { new ActorData { Value = "Actor 5" }, new ActorData { Value = "Actor 6" } },
                    AgeRating = 18,
                    PosterUrl = "http://example.com/poster3.jpg"
                }
            }    
        ];

        IMovieEventQuery query = new FakeMovieEventQuery(data);
        SearchMovieEventsInTimerange useCase = new SearchMovieEventsInTimerange(query);
        SearchMovieEventsInTimerangeInput input = new SearchMovieEventsInTimerangeInput(fromDate, toDate);

        // Act
        IReadOnlyList<MovieEventData> result = await useCase.Execute(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.Movie.Title == "Movie 1");
        Assert.Contains(result, e => e.Movie.Title == "Movie 3");
    }

    private sealed class FakeMovieEventQuery(IReadOnlyList<MovieEventData> data) : IMovieEventQuery
    {
        private readonly IReadOnlyList<MovieEventData> _data = data;

        public Task<IReadOnlyList<MovieEventData>> Fetch(Expression<Func<MovieEventData, bool>> filter)
        {
            Func<MovieEventData, bool> compiledFilter = filter.Compile();
            IReadOnlyList<MovieEventData> result = _data.Where(compiledFilter).ToList();
            return Task.FromResult(result);
        }
    }
}