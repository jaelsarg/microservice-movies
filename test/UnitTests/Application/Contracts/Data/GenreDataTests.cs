using Howestprime.Movies.Application.Contracts.Data;

namespace UnitTests.Application.Contracts.Data;

public sealed class GenreDataTests
{
    [Fact]
    public void Create_WithValues_ShouldSetProperties()
    {
        // Arrange
        string value = "Leonardo DiCaprio";

        // Act
        GenreData genreData = new GenreData
        {
            Value = value
        };

        // Assert
        Assert.Equal(value, genreData.Value);
    }

    [Fact]
    public void Create_WithDefaults_ShouldInitializeValues()
    {
        // Arrange & Act
        GenreData genreData = new GenreData();

        // Assert
        Assert.Equal(string.Empty, genreData.Value);
    }
}