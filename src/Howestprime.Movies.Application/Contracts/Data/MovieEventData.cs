namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record MovieEventData
{
    public Guid Id { get; init; }
    public DateTime Showtime { get; init; } = DateTime.MinValue;
    public int Capacity { get; init; } = 0;
    public RoomData Room { get; init; } = default!;
    public MovieData Movie { get; init; } = default!;
}