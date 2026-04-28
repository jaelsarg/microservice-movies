using Howestprime.Movies.Application.Contracts.Data;

namespace UnitTests.Application.Contracts.Data;

public sealed class ActorDataTests
{
    [Fact]
    public void Create_WithValues_ShouldSetProperties()
    {
        // Arrange
        string value = "Leonardo DiCaprio";

        // Act
        ActorData actorData = new ActorData
        {
            Value = value
        };

        // Assert
        Assert.Equal(value, actorData.Value);
    }

    [Fact]
    public void Create_WithDefaults_ShouldInitializeValues()
    {
        // Arrange & Act
        ActorData actorData = new ActorData();

        // Assert
        Assert.Equal(string.Empty, actorData.Value);
    }
}