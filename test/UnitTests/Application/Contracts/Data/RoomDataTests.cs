using Howestprime.Movies.Application.Contracts.Data;

namespace UnitTests.Application.Contracts.Data;

public sealed class RoomDataTests
{
    [Fact]
    public void Create_WithValues_ShouldSetProperties()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = "Room 1";
        int capacity = 100;

        // Act
        RoomData roomData = new RoomData
        {
            Id = id,
            Name = name,
            Capacity = capacity
        };

        // Assert
        Assert.Equal(id, roomData.Id);
        Assert.Equal(name, roomData.Name);
        Assert.Equal(capacity, roomData.Capacity);
    }

    [Fact]
    public void Create_WithDefaults_ShouldInitializeValues()
    {
        // Arrange & Act
        RoomData roomData = new RoomData();

        // Assert
        Assert.Equal(Guid.Empty, roomData.Id);
        Assert.Equal(string.Empty, roomData.Name);
        Assert.Equal(0, roomData.Capacity);
    }
}