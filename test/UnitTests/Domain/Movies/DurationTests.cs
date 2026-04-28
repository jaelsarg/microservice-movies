using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class DurationTests
{
    [Fact]
    public void Create_WithValidDuration_Succeeds()
    {
        // Arrange
        int duration = 120;

        // Act
        var durationValue = Duration.Create(duration);

        // Assert
        Assert.Equal(duration, durationValue.Value);
    }

    [Fact]
    public void Create_WithInvalidDuration_ShouldThrow()
    {
        // Arrange
        int duration = -5;

        // Act
        Action act = () => Duration.Create(duration);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}