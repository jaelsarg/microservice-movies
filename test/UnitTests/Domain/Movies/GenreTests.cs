using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class GenreTests
{
    [Fact]
    public void Create_WithValidGenre_Succeeds()
    {
        // Arrange
        string genre = "Action";

        // Act
        var genreValue = Genre.Create(genre);

        // Assert
        Assert.Equal(genre, genreValue.Value);
    }

    [Fact]
    public void Create_WithInvalidGenre_ShouldThrow()
    {
        // Arrange
        string genre = "";

        // Act
        Action act = () => Genre.Create(genre);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}