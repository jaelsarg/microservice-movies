using System.Linq.Expressions;
using Aornis;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;

namespace UnitTests.Application.Movies;

public sealed class FindMovieByIdTests
{
    [Fact]
    public async Task Execute_WithValidId_ShouldReturnMovie()
    {
        // Arrange
        Guid movieId = Guid.NewGuid();
        List<MovieData> data =
        [
            new MovieData
            {
                Id = movieId,
                Title = "Inception",
                Description = "Dream heist",
                ReleaseYear = 2010,
                Genres = [ new GenreData { Value = "action" }, new GenreData { Value = "sci-fi" } ],
                Actors = [ new ActorData { Value = "Leonardo DiCaprio" } ],
                AgeRating = 13,
                Duration = 148,
                PosterUrl = "inception.jpg"
            },
            new MovieData
            {
                Id = Guid.NewGuid(),
                Title = "The Notebook",
                Description = "Romance drama",
                ReleaseYear = 2004,
                Genres = [ new GenreData { Value = "romance" } ],
                Actors = [ new ActorData { Value = "Rachel McAdams" } ],
                AgeRating = 13,
                Duration = 123,
                PosterUrl = "notebook.jpg"
            }
        ];

        ISearchMovieCatalogQuery query = new FakeSearchMovieCatalogQuery(data);
        FindMovieById useCase = new(query);
        FindMovieByIdInput input = new(movieId);

        // Act
        Optional<MovieData> result = await useCase.Execute(input);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(movieId, result.Value.Id);
    }

    private sealed class FakeSearchMovieCatalogQuery(IReadOnlyList<MovieData> data) : ISearchMovieCatalogQuery
    {
        private readonly IReadOnlyList<MovieData> _data = data;

        public Task<IReadOnlyList<MovieData>> Fetch(Expression<Func<MovieData, bool>> movieFilter)
        {
            IReadOnlyList<MovieData> filtered = _data.AsQueryable().Where(movieFilter).ToList();
            return Task.FromResult(filtered);
        }
    }
}