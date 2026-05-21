using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
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
        ReleaseYear releaseYear = ReleaseYear.Create(2010);
        Duration duration = Duration.Create(148);
        List<Genre> genres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi") };
        List<Actor> actors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") };
        AgeRating ageRating = AgeRating.Create(13);
        PosterUrl posterUrl = PosterUrl.Create("https://example.com/inception.jpg");

        // Act
        var movie = Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl);

        // Assert
        Assert.Equal(title, movie.Title);
        Assert.Equal(description, movie.Description);
        Assert.Equal(releaseYear.Value, movie.ReleaseYear.Value);
        Assert.Equal(duration.Value, movie.Duration.Value);
        Assert.Equal(genres.Count(), movie.Genres.Count());
        Assert.Equal(actors.Count(), movie.Actors.Count());
        Assert.Equal(ageRating.Value, movie.AgeRating.Value);
        Assert.Equal(posterUrl.Value, movie.PosterUrl.Value);
    }

    [Fact]
    public void Create_WithValidInputAndId_Succeeds()
    {
        // Arrange
        MovieId movieId = EntityId.New<MovieId>();
        string title = "Inception";
        string description = "A mind-bending thriller about dreams within dreams.";
        ReleaseYear releaseYear = ReleaseYear.Create(2010);
        Duration duration = Duration.Create(148);
        List<Genre> genres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi") };
        List<Actor> actors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") };
        AgeRating ageRating = AgeRating.Create(13);
        PosterUrl posterUrl = PosterUrl.Create("https://example.com/inception.jpg");

        // Act
        var movie = Movie.Create(title, description, releaseYear, duration, genres, actors, ageRating, posterUrl, movieId);

        // Assert
        Assert.Equal(movieId, movie.Id);
        Assert.Equal(title, movie.Title);
        Assert.Equal(description, movie.Description);
        Assert.Equal(releaseYear.Value, movie.ReleaseYear.Value);
        Assert.Equal(duration.Value, movie.Duration.Value);
        Assert.Equal(genres.Count(), movie.Genres.Count());
        Assert.Equal(actors.Count(), movie.Actors.Count());
        Assert.Equal(ageRating.Value, movie.AgeRating.Value);
        Assert.Equal(posterUrl.Value, movie.PosterUrl.Value);
    }

    [Fact]
    public void Create_WithInvalidTitle_ShouldThrow()
    {
        // Arrange
        string title = "";
        string description = "A mind-bending thriller about dreams within dreams.";
        ReleaseYear releaseYear = ReleaseYear.Create(2010);
        Duration duration = Duration.Create(148);
        List<Genre> genres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi") };
        List<Actor> actors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") };
        AgeRating ageRating = AgeRating.Create(13);
        PosterUrl posterUrl = PosterUrl.Create("https://example.com/inception.jpg");

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
        ReleaseYear releaseYear = ReleaseYear.Create(2010);
        Duration duration = Duration.Create(148);
        List<Genre> genres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi") };
        List<Actor> actors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") };
        AgeRating ageRating = AgeRating.Create(13);
        PosterUrl posterUrl = PosterUrl.Create("https://example.com/inception.jpg");

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
        ReleaseYear releaseYear = ReleaseYear.Create(2030);
        Duration duration = Duration.Create(148);
        List<Genre> genres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi") };
        List<Actor> actors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") };
        AgeRating ageRating = AgeRating.Create(13);
        PosterUrl posterUrl = PosterUrl.Create("https://example.com/inception.jpg");

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
            ReleaseYear.Create(2010),
            Duration.Create(148),
            new() { Genre.Create("Action"), Genre.Create("Sci-Fi") },
            new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") },
            AgeRating.Create(13),
            PosterUrl.Create("https://example.com/inception.jpg")
        );

        MovieId movieId = movie.Id;
        string newTitle = "Inception 2";
        string newDescription = "The sequel to the mind-bending thriller.";
        ReleaseYear newReleaseYear = ReleaseYear.Create(2022);
        Duration newDuration = Duration.Create(150);
        List<Genre> newGenres = new() { Genre.Create("Action"), Genre.Create("Sci-Fi"), Genre.Create("Thriller") };
        List<Actor> newActors = new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt"), Actor.Create("Elliot Page") };
        AgeRating newAgeRating = AgeRating.Create(15);
        PosterUrl newPosterUrl = PosterUrl.Create("https://example.com/inception2.jpg");

        // Act
        movie.ChangeDetails(newTitle, newDescription, newReleaseYear, newDuration, newGenres, newActors, newAgeRating, newPosterUrl);

        // Assert
        Assert.Equal(movieId, movie.Id);
        Assert.Equal(newTitle, movie.Title);
        Assert.Equal(newDescription, movie.Description);
        Assert.Equal(newReleaseYear.Value, movie.ReleaseYear.Value);
        Assert.Equal(newDuration.Value, movie.Duration.Value);
        Assert.Equal(newGenres.Count(), movie.Genres.Count());
        Assert.Equal(newActors.Count(), movie.Actors.Count());
        Assert.Equal(newAgeRating.Value, movie.AgeRating.Value);
        Assert.Equal(newPosterUrl.Value, movie.PosterUrl.Value);
    }

    [Fact]
    public void ChangeDetails_WithInvalidTitle_ShouldThrow()
    {
        // Arrange
        Movie movie = Movie.Create(
            "Inception",
            "A mind-bending thriller about dreams within dreams.",
            ReleaseYear.Create(2010),
            Duration.Create(148),
            new() { Genre.Create("Action"), Genre.Create("Sci-Fi") },
            new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") },
            AgeRating.Create(13),
            PosterUrl.Create("https://example.com/inception.jpg")
        );

        string newTitle = "";

        // Act
        Action act = () => movie.ChangeDetails(newTitle, movie.Description, movie.ReleaseYear, movie.Duration, movie.Genres.Select(g => g).ToList(), movie.Actors.Select(a => a).ToList(), movie.AgeRating, movie.PosterUrl);

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
            ReleaseYear.Create(2010),
            Duration.Create(148),
            new() { Genre.Create("Action"), Genre.Create("Sci-Fi") },
            new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") },
            AgeRating.Create(13),
            PosterUrl.Create("https://example.com/inception.jpg")
        );

        string newDescription = "";

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, newDescription, movie.ReleaseYear, movie.Duration, movie.Genres.Select(g => g).ToList(), movie.Actors.Select(a => a).ToList(), movie.AgeRating, movie.PosterUrl);

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
            ReleaseYear.Create(2010),
            Duration.Create(148),
            new() { Genre.Create("Action"), Genre.Create("Sci-Fi") },
            new() { Actor.Create("Leonardo DiCaprio"), Actor.Create("Joseph Gordon-Levitt") },
            AgeRating.Create(13),
            PosterUrl.Create("https://example.com/inception.jpg")
        );

        ReleaseYear newReleaseYear = ReleaseYear.Create(2030);

        // Act
        Action act = () => movie.ChangeDetails(movie.Title, movie.Description, newReleaseYear, movie.Duration, movie.Genres.Select(g => g).ToList(), movie.Actors.Select(a => a).ToList(), movie.AgeRating, movie.PosterUrl);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}