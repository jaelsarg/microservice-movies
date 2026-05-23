using Microsoft.Extensions.Logging;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Seeders;

public class DomainDbSeeder(DomainDbContext context, ILogger<DomainDbSeeder> _logger)
{
    public async Task Seed()
    {
        List<Room> rooms = new List<Room>
        {
            Room.Create("Blue Room", 100, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-71db-8a1a-ec7569492999"))),
            Room.Create("Yellow Room", 80, EntityId.New<RoomId>(Guid.Parse("019d059e-d220-75fe-b936-0a97cd75216e")))
        };

        if (!await context.Rooms.AnyAsync())
        {
            _logger.LogInformation("Seeding domain database with rooms...");
            context.Rooms.AddRange(rooms);
        }

        await context.SaveChangesAsync();

        _logger.LogInformation("Seeded domain database with rooms.");
    }
}
