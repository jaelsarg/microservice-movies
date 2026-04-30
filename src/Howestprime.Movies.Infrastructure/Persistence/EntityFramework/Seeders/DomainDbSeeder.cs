using Microsoft.Extensions.Logging;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Movies;

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
        List<Movie> movies = new List<Movie>
        {
            Movie.Create("Shadow Protocol", "An elite hacker uncovers a global conspiracy.", 2022, 118, new List<string> { "Thriller", "Action" }, new List<string> { "John Carter", "Elena Voss" }, 16, "https://example.com/posters/shadow-protocol.jpg", EntityId.New<MovieId>(Guid.Parse("c1f4c9d1-7b6a-4f5e-9f3b-2a7d9d8c1a01"))),
            Movie.Create("Echoes of Tomorrow", "Time travelers try to fix a broken future.", 2023, 130, new List<string> { "Sci-Fi", "Drama" }, new List<string> { "Mila Hart", "Jason Reed" }, 12, "https://example.com/posters/echoes.jpg", EntityId.New<MovieId>(Guid.Parse("a8b2e7c3-1d44-4f91-9b0f-6c0f2b9a7c22"))),
            Movie.Create("Crimson Night", "A detective hunts a serial killer in a dark city.", 2021, 105, new List<string> { "Crime", "Mystery" }, new List<string> { "Liam Stone", "Rachel Grey" }, 18, "https://example.com/posters/crimson-night.jpg", EntityId.New<MovieId>(Guid.Parse("f3d91b75-5d3a-4cbb-8c4f-1d2a1b7c8e33"))),
            Movie.Create("Ocean's Secret", "Divers uncover an ancient underwater civilization.", 2020, 112, new List<string> { "Adventure", "Fantasy" }, new List<string> { "Chris Bay", "Nina Shore" }, 10, "https://example.com/posters/ocean-secret.jpg", EntityId.New<MovieId>(Guid.Parse("9a7f2d6c-0e5c-4b2f-9c55-3a1b7c9e4f44"))),
            Movie.Create("Steel Horizon", "A war between humans and AI reaches its peak.", 2024, 140, new List<string> { "Action", "Sci-Fi" }, new List<string> { "Victor Kane", "Sara Lin" }, 16, "https://example.com/posters/steel-horizon.jpg", EntityId.New<MovieId>(Guid.Parse("bb4e7c81-6c9d-4f4d-bdb3-0d5f2a9e8c55"))),
            Movie.Create("Golden Fields", "A heartfelt story about family and legacy.", 2019, 95, new List<string> { "Drama" }, new List<string> { "Emma Clarke", "Daniel Frost" }, 6, "https://example.com/posters/golden-fields.jpg", EntityId.New<MovieId>(Guid.Parse("de2f3c49-8f2b-4e0c-8d9a-4c1f2e7b6d66"))),
            Movie.Create("Neon Streets", "A cyberpunk city hides secrets in every alley.", 2025, 125, new List<string> { "Sci-Fi", "Crime" }, new List<string> { "Axel Ray", "Luna Vega" }, 16, "https://example.com/posters/neon-streets.jpg", EntityId.New<MovieId>(Guid.Parse("71c5e6a2-2d1f-4c7e-a0d4-9e2a7b3f8a77"))),
            Movie.Create("Frozen Peak", "Climbers face deadly conditions on a frozen mountain.", 2018, 102, new List<string> { "Adventure", "Thriller" }, new List<string> { "Mark Ice", "Sophie Hill" }, 12, "https://example.com/posters/frozen-peak.jpg", EntityId.New<MovieId>(Guid.Parse("8c1b4e2f-3a9d-4d2b-bf6c-5f7a9d2e9b88"))),
            Movie.Create("Laugh Track", "A struggling comedian finds unexpected fame.", 2021, 98, new List<string> { "Comedy" }, new List<string> { "Tom Bright", "Ella Joy" }, 10, "https://example.com/posters/laugh-track.jpg", EntityId.New<MovieId>(Guid.Parse("5d7e2a91-0c4b-4f6d-9b8a-6c2e1f7d3c99"))),
            Movie.Create("Silent Orbit", "Astronauts encounter a mysterious force in deep space.", 2022, 115, new List<string> { "Sci-Fi", "Horror" }, new List<string> { "Neil Cross", "Ivy Star" }, 16, "https://example.com/posters/silent-orbit.jpg", EntityId.New<MovieId>(Guid.Parse("2f9b6c3d-7e1a-4b8f-9d2c-8a1e7f6b4d00")))
        };

        if (!await context.Rooms.AnyAsync())
        {
            _logger.LogInformation("Seeding domain database with rooms...");
            context.Rooms.AddRange(rooms);
        }

        if (!await context.Movies.AnyAsync())
        {
            _logger.LogInformation("Seeding domain database with movies...");
            context.Movies.AddRange(movies);
        }

        await context.SaveChangesAsync();

        _logger.LogInformation("Seeded domain database with rooms and movies.");
    }
}
