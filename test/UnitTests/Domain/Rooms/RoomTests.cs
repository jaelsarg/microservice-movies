using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;

namespace UnitTests.Domain.Rooms;

public sealed class RoomTests
{
    [Fact]
    public void Create_WithValidInput_ShouldCreateRoom()
    {
        // Arrange
        string name = "Room 1";
        int capacity = 100;

        // Act
        Room room = Room.Create(name, capacity);

        // Assert
        Assert.Equal(name, room.Name);
        Assert.Equal(capacity, room.Capacity); 
    }

    [Fact]
    public void Create_WithValidInputAndId_ShouldCreateRoom()
    {
        // Arrange
        RoomId roomId = EntityId.New<RoomId>();
        string name = "Room 1";
        int capacity = 100;

        // Act
        Room room = Room.Create(name, capacity, roomId);

        // Assert
        Assert.Equal(roomId, room.Id);
        Assert.Equal(name, room.Name);
        Assert.Equal(capacity, room.Capacity);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrow()
    {
        // Arrange
        string name = "";
        int capacity = 100;

        // Act
        Action act = () => Room.Create(name, capacity);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeCapacity_ShouldThrow()
    {
        // Arrange
        string name = "Room 1";
        int capacity = -10;

        // Act
        Action act = () => Room.Create(name, capacity);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}