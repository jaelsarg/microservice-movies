using Howestprime.Movies.Domain.MovieEvents;

namespace UnitTests.Domain.MovieEvents;

public sealed class MovieEventAssertsTests
{
    [Fact]
    public void EnsureShowTimeIsAt15hOr19h_WithValidTime_ShouldNotThrow()
    {
        // Arrange
        DateTime validTime = DateTime.Now.Date.AddHours(15);

        // Act
        Action act = () => MovieEventAsserts.EnsureShowTimeIsAt15hOr19h(validTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowTimeIsAt15hOr19h_WithInvalidTime_ShouldThrow()
    {
        // Arrange
        DateTime invalidTime = DateTime.Now.Date.AddHours(14);

        // Act
        Action act = () => MovieEventAsserts.EnsureShowTimeIsAt15hOr19h(invalidTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithFutureTime_ShouldNotThrow()
    {
        // Arrange
        DateTime futureTime = DateTime.Now.AddDays(1);

        // Act
        Action act = () => MovieEventAsserts.EnsureShowtimeIsInTheFuture(futureTime);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureShowtimeIsInTheFuture_WithPastTime_ShouldThrow()
    {
        // Arrange
        DateTime pastTime = DateTime.Now.AddDays(-1);

        // Act
        Action act = () => MovieEventAsserts.EnsureShowtimeIsInTheFuture(pastTime);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}