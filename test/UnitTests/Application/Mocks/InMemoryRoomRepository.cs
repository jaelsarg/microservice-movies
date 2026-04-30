using Aornis;
using Howestprime.Movies.Domain.Rooms;

namespace UnitTests.Application.Mocks;

public sealed class InMemoryRoomRepository : IRoomRepository
{
    private readonly HashSet<Room> _rooms = [];

    public Task<Optional<Room>> ById(RoomId id)
    {
        return Task.FromResult(
            Optional.Of(_rooms.FirstOrDefault(room => room.Id.Equals(id)))
        );
    }

    public Task<bool> Exists(RoomId id)
    {
        return Task.FromResult(
            _rooms.Any(movie => movie.Id.Equals(id))
        );
    }

    public Task Remove(Room aggregateRoot)
    {
        return Task.FromResult(
            _rooms.Remove(aggregateRoot)
        );
    }

    public Task Save(Room aggregateRoot)
    {
        _rooms.Add(aggregateRoot);
        return Task.CompletedTask;
    }
}