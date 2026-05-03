using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Shared;

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
    public void Create_WithValidInputAndId_Succeeds()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        string title = "Inception";
        string description = "A mind-bending thriller about dreams within dreams.";
        int releaseYear = 2010;
        int duration = 148;
        List<string> genres = new() { "Action", "Sci-Fi" };
        List<string> actors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" };
        int ageRating = 13;
        string posterUrl = "https://example.com/inception.jpg";

        // Act
        var movie = Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl, movieId);

        // Assert
        Assert.Equal(movieId, movie.Id);
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

    [Fact]
    public void Create_WithReleaseYearInFuture_ShouldThrow()
    {
        // Arrange
        string title = "Inception";
        string description = "A mind-bending thriller about dreams within dreams.";
        int releaseYear = 2030;
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
    public void ChangeDetails_WithValidInput_Succeeds()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        MovieId movieId = movie.Id;
        string newTitle = "Inception 2";
        string newDescription = "The sequel to the mind-bending thriller.";
        int newReleaseYear = 2022;
        int newDuration = 150;
        List<string> newGenres = new() { "Action", "Sci-Fi", "Thriller" };
        List<string> newActors = new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page" };
        int newAgeRating = 15;
        string newPosterUrl = "https://example.com/inception2.jpg";

        // Act
        movie.ChangeDetails(newTitle, newDescription, newReleaseYear, newDuration, newGenres, newActors, newAgeRating, newPosterUrl);

        // Assert
        Assert.Equal(movieId, movie.Id);
        Assert.Equal(newTitle, movie.Title);
        Assert.Equal(newDescription, movie.Description);
        Assert.Equal(newReleaseYear, movie.ReleaseYear.Value);
        Assert.Equal(newDuration, movie.Duration.Value);
        Assert.Equal(newGenres.Count(), movie.Genres.Count());
        Assert.Equal(newActors.Count(), movie.Actors.Count());
        Assert.Equal(newAgeRating, movie.AgeRating.Value);
        Assert.Equal(newPosterUrl, movie.PosterUrl.Value);
    }

    [Fact]
    public void ChangeDetails_WithInvalidTitle_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        string newTitle = "";

        // Act
        Action act = () => movie.ChangeDetails(newTitle, movie.Description, movie.ReleaseYear.Value, movie.Duration.Value, movie.Genres.Select(g => g.Value).ToList(), movie.Actors.Select(a => a.Value).ToList(), movie.AgeRating.Value, movie.PosterUrl.Value);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ChangeDetails_WithInvalidDescription_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        string newDescription = "";

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, newDescription, movie.ReleaseYear.Value, movie.Duration.Value, movie.Genres.Select(g => g.Value).ToList(), movie.Actors.Select(a => a.Value).ToList(), movie.AgeRating.Value, movie.PosterUrl.Value);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ChangeDetails_WithReleaseYearInFuture_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        int newReleaseYear = 2030;

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, movie.Description, newReleaseYear, movie.Duration.Value, movie.Genres.Select(g => g.Value).ToList(), movie.Actors.Select(a => a.Value).ToList(), movie.AgeRating.Value, movie.PosterUrl.Value);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ChangeDetails_WithNegativeAgeRating_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        int newAgeRating = -1;

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, movie.Description, movie.ReleaseYear.Value, movie.Duration.Value, movie.Genres.Select(g => g.Value).ToList(), movie.Actors.Select(a => a.Value).ToList(), newAgeRating, movie.PosterUrl.Value);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ChangeDetails_WithNegativeDuration_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            2010,
            148,
            new() { "Action", "Sci-Fi" },
            new() { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
            13,
            "https://example.com/inception.jpg"
        );

        int newDuration = -1;

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, movie.Description, movie.ReleaseYear.Value, newDuration, movie.Genres.Select(g => g.Value).ToList(), movie.Actors.Select(a => a.Value).ToList(), movie.AgeRating.Value, movie.PosterUrl.Value);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}