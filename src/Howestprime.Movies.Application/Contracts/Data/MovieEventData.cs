namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record MovieEventData
{
    public Guid Id { get; init; }
    public DateTime Showtime { get; init; }
    public int Capacity { get; init; }
    public RoomData Room { get; init; }
    public MovieData Movie { get; init; }
}