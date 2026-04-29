using Howestprime.Movies.Application.Movies;
using UnitTests.Application.Shared;

namespace UnitTests.Application.Movies;

public sealed class RegisterMovieTests : ApplicationTestBase
{
    private readonly RegisterMovie _registerMovie;

    public RegisterMovieTests()
    {
        _registerMovie = new RegisterMovie(UnitOfWork, CreateLogger<RegisterMovie>());
    }
    
    [Fact]
    public async Task Execute_WithValidMovie_ShouldRegisterMovie()
    {
        // Arrange
        RegisterMovieInput input = new(
            Title: "Inception",
            Description: "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            Duration: 148,
            Genres: new List<string> { "Action", "Adventure", "Sci-Fi" },
            ReleaseYear: 2010,
            Actors: new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page" },
            AgeRating: 13,
            PosterUrl: "https://example.com/inception.jpg"
        );

        // Act
        string id = await _registerMovie.Execute(input);

        // Assert
        Assert.False(string.IsNullOrEmpty(id), "Movie ID should not be null or empty.");
    }

    [Fact]
    public async Task Execute_WithInvalidInput_ShouldThrowException()
    {
        // Arrange
        RegisterMovieInput input = new(
            Title: "",
            Description: "",
            Duration: -1,
            Genres: new List<string>(),
            ReleaseYear: 1800,
            Actors: new List<string>(),
            AgeRating: -1,
            PosterUrl: "invalid-url"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _registerMovie.Execute(input));
    }
}