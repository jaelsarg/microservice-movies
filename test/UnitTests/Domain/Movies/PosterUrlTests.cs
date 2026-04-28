using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class PosterUrlTests
{
    [Fact]
    public void Create_WithValidPosterUrl_Succeeds()
    {
        // Arrange
        string posterUrl = "https://example.com/poster.jpg";

        // Act
        var posterUrlValue = PosterUrl.Create(posterUrl);

        // Assert
        Assert.Equal(posterUrl, posterUrlValue.Value);
    }

    [Fact]
    public void Create_WithInvalidPosterUrl_ShouldThrow()
    {
        // Arrange
        string posterUrl = "";


        // Act
        Action act = () => PosterUrl.Create(posterUrl);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}