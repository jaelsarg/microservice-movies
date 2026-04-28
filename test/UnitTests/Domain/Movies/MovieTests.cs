using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class MovieTests
{
    [Fact]
    public void Create_WithValidInput_Succeeds()
    {
        // Arrange
        string title = "Inception";
        string description = "A mind-bending thriller about dreams within dreams.";
        int releaseYear = 2010;
        int duration = 148;
        List<string> genres = new() { "Action", "Sci-Fi" };
        List<string> actors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" };
        int ageRating = 13;
        string posterUrl = "https://example.com/inception.jpg";

        // Act
        var movie = Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Equal(title, movie.Title);
        Assert.Equal(description, movie.Description);
        Assert.Equal(releaseYear, movie.ReleaseYear.Value);
        Assert.Equal(duration, movie.Duration.Value);
        Assert.Equal(genres.Count(), movie.Genres.Count());
        Assert.Equal(actors.Count(), movie.Actors.Count());
        Assert.Equal(ageRating, movie.AgeRating.Value);
        Assert.Equal(posterUrl, movie.PosterUrl.Value);
    }

    [Fact]
    public void Create_WithInvalidTitle_ShouldThrow()
    {
        // Arrange
        string title = "";
        string description = "A mind-bending thriller about dreams within dreams.";
        int releaseYear = 2010;
        int duration = 148;
        List<string> genres = new() { "Action", "Sci-Fi" };
        List<string> actors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" };
        int ageRating = 13;
        string posterUrl = "https://example.com/inception.jpg";

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithInvalidDescription_ShouldThrow()
    {
        // Arrange
        string title = "Inception";
        string description = "";
        int releaseYear = 2010;
        int duration = 148;
        List<string> genres = new() { "Action", "Sci-Fi" };
        List<string> actors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" };
        int ageRating = 13;
        string posterUrl = "https://example.com/inception.jpg";

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeAgeRating_ShouldThrow()
    {
        // Arrange
        string title = "Inception";
        string description = "A mind-bending thriller about dreams within dreams.";
        int releaseYear = 2010;
        int duration = 148;
        List<string> genres = new() { "Action", "Sci-Fi" };
        List<string> actors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" };
        int ageRating = -1;
        string posterUrl = "https://example.com/inception.jpg";

        // Act
        Action act = () => Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}