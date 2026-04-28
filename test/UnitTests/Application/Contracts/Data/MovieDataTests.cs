using Howestprime.Movies.Application.Contracts.Data;

namespace UnitTests.Application.Contracts.Data;

public sealed class MovieDataTests
{
    [Fact]
    public void Create_WithValues_ShouldSetProperties()
    {
        // Arrange
        string PosterUrl = "https://example.com/posters/inception.jpg";
        string Title = "Inception";
        List<GenreData> Genres = new List<GenreData>
        {
            new GenreData { Value = "Sci-Fi" },
            new GenreData { Value = "Action" },
            new GenreData { Value = "Thriller" }
        };
        List<ActorData> Actors = new List<ActorData>
        {
            new ActorData { Value = "Leonardo DiCaprio" },
            new ActorData { Value = "Joseph Gordon-Levitt" },
            new ActorData { Value = "Elliot Page" }
        };
        int AgeRating = 13;
        int ReleaseYear = 2010;
        int Duration = 148;
        string Description = "A skilled thief uses dream-sharing technology to plant an idea into the mind of a CEO.";

        // Act
        MovieData movieData = new MovieData
        {
            PosterUrl = PosterUrl,
            Title = Title,
            Genres = Genres,
            Actors = Actors,
            AgeRating = AgeRating,
            ReleaseYear = ReleaseYear,
            Duration = Duration,
            Description = Description
        };

        // Assert
        Assert.Equal(PosterUrl, movieData.PosterUrl);
        Assert.Equal(Title, movieData.Title);
        Assert.Equal(Genres, movieData.Genres);
        Assert.Equal(Actors, movieData.Actors);
        Assert.Equal(AgeRating, movieData.AgeRating);
        Assert.Equal(ReleaseYear, movieData.ReleaseYear);
        Assert.Equal(Duration, movieData.Duration);
        Assert.Equal(Description, movieData.Description);
    }

    [Fact]
    public void Create_WithDefaults_ShouldInitializeValues()
    {
        // Arrange & Act
        MovieData movieData = new MovieData();

        // Assert
        Assert.Equal(string.Empty, movieData.PosterUrl);
        Assert.Equal(string.Empty, movieData.Title);
        Assert.Equal(0, movieData.AgeRating);
        Assert.Equal(0, movieData.ReleaseYear);
        Assert.Equal(0, movieData.Duration);
        Assert.Equal(string.Empty, movieData.Description);
    }
}