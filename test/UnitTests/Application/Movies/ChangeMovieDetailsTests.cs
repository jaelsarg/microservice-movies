using Aornis;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.Movies;
using UnitTests.Application.Shared;

namespace UnitTests.Application.Movies;

public sealed class ChangeMovieDetailsTests : ApplicationTestBase
{
    private readonly RegisterMovie _registerMovie;
    private readonly ChangeMovieDetails _changeMovieDetails;

    public ChangeMovieDetailsTests()
    {
        _registerMovie = new RegisterMovie(UnitOfWork, CreateLogger<RegisterMovie>());
        _changeMovieDetails = new ChangeMovieDetails(UnitOfWork, CreateLogger<ChangeMovieDetails>());
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldChangeMovieDetails()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            2020,
            148,
            new List<string> { "Action", "Adventure", "Sci-Fi" },
            new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page" },
            13,
            "https://example.com/inception.jpg"
        );
        await UnitOfWork.Repo<IMovieRepository>().Save(movie);
    
        ChangeMovieDetailsInput changeInput = new(
            Id: movie.Id.Value,
            Title: "Inception (Edited)",
            Description: "An edited description.",
            Duration: 150,
            Genres: new List<string> { "Action", "Sci-Fi" },
            ReleaseYear: 2010,
            Actors: new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            AgeRating: 13,
            PosterUrl: "https://example.com/inception_edited.jpg"
        );

        // Act
        await _changeMovieDetails.Execute(changeInput);

        // Assert
        Optional<Movie> optionalMovie = await UnitOfWork.Repo<IMovieRepository>().ById(new MovieId(changeInput.Id));
        Movie updated = optionalMovie.Value;

        Assert.Equal(changeInput.Title, updated.Title);
        Assert.Equal(changeInput.Description, updated.Description);
        Assert.Equal(changeInput.Duration, updated.Duration.Value);
        Assert.Equal(changeInput.Genres.Count, updated.Genres.Count);
        Assert.Equal(changeInput.Genres[0], updated.Genres[0].Value);
        Assert.Equal(changeInput.ReleaseYear, updated.ReleaseYear.Value);
        Assert.Equal(changeInput.Actors.Count, updated.Actors.Count);
        Assert.Equal(changeInput.Actors[0], updated.Actors[0].Value);
        Assert.Equal(changeInput.AgeRating, updated.AgeRating.Value);
    }
}