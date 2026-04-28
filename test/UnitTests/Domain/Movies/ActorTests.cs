using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace UnitTests.Domain.Movies;

public sealed class ActorTests
{
    [Fact]
    public void Create_WithValidName_Succeeds()
    {
        // Arrange
        string name = "John Doe";

        // Act
        var actor = Actor.Create(name);

        // Assert
        Assert.Equal(name, actor.Value);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrow()
    {
        // Arrange
        string name = "";

        // Act
        Action act = () => Actor.Create(name);

        // Act & Assert
        Assert.Throws<ArgumentException>(act);
    }
}