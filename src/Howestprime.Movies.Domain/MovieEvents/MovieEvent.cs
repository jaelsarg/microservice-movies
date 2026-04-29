using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.MovieEvents;

public readonly record struct MovieEventId(Guid Value) : IEntityId;

public sealed class MovieEvent : AggregateRoot<MovieEventId>
{
    public MovieId MovieId { get; private set; }
    public RoomId RoomId { get; private set; }
    public DateTime Showtime { get; private set; }
    public int Capacity { get; private set; }

    private MovieEvent(
        MovieEventId id,
        MovieId movieId,
        RoomId roomId,
        DateTime showtime,
        int capacity
    ) : base(id)
    {
        MovieId = movieId;
        RoomId = roomId;
        Showtime = showtime;
        Capacity = capacity;
    }
    
    #pragma warning disable CS8618
    private MovieEvent() :base(default!) {}
    #pragma warning restore CS8618

    public static MovieEvent Create(
        MovieId movieId,
        RoomId roomId,
        DateTime showtime,
        int capacity
    )
    {
        MovieEvent movieEvent = new(
            EntityId.New<MovieEventId>(),
            movieId,
            roomId,
            showtime,
            capacity
        );

        movieEvent.ValidateState();

        return movieEvent;
    }

    public override void ValidateState()
    {
        MovieEventAsserts.EnsureShowTimeIsAt15hOr19h(Showtime);
        MovieEventAsserts.EnsureShowtimeIsInTheFuture(Showtime);
    }
}