using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class AgeRatingTests
{
    [Fact]
    public void Create_WithValidAge_Succeeds()
    {
        // Arrange
        int age = 25;

        // Act
        var ageRating = AgeRating.Create(age);

        // Assert
        Assert.Equal(age, ageRating.Value);
    }

    [Fact]
    public void Create_WithInvalidAge_ShouldThrow()
    {
        // Arrange
        int age = -5;

        // Act
        Action act = () => AgeRating.Create(age);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}