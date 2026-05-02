namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record RoomData
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Capacity { get; init; } = 0;
}