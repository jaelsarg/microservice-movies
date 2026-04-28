using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class ReleaseYearTests
{
    [Fact]
    public void Create_WithValidReleaseYear_Succeeds()
    {
        // Arrange
        int releaseYear = 2020;

        // Act
        var releaseYearValue = ReleaseYear.Create(releaseYear);

        // Assert
        Assert.Equal(releaseYear, releaseYearValue.Value);
    }

    [Fact]
    public void Create_WithReleaseYearInFuture_ShouldThrow()
    {
        // Arrange
        int releaseYear = 2050;

        // Act
        Action act = () => ReleaseYear.Create(releaseYear);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeReleaseYear_ShouldThrow()
    {
        // Arrange
        int releaseYear = -1;

        // Act
        Action act = () => ReleaseYear.Create(releaseYear);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}