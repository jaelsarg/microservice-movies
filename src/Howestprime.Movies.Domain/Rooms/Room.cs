using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Rooms;

public readonly record struct RoomId(Guid Value) : IEntityId;

public sealed class Room : AggregateRoot<RoomId>
{
    public string Name { get; private set; }
    public int Capacity { get; private set; }

    private Room(
        RoomId id,
        string name,
        int capacity
    ) : base(id)
    {
        Name = name;
        Capacity = capacity;
    }

    #pragma warning disable CS8618
    private Room() :base(default!) {}
    #pragma warning restore CS8618

    public static Room Create(
        string name,
        int capacity
    )
    {
        Room room = new(
            EntityId.New<RoomId>(),
            name,
            capacity
        );

        room.ValidateState();

        return room;
    }

    public override void ValidateState()
    {
        throw new NotImplementedException();
    }
}