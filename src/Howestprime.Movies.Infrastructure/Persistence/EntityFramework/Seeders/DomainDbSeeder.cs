using Microsoft.Extensions.Logging;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;

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
            Movie.Create("Shadow Protocol", "An elite hacker uncovers a global conspiracy.", ReleaseYear.Create(2022), Duration.Create(118), new List<Genre> { Genre.Create("Thriller"), Genre.Create("Action") }, new List<Actor> { Actor.Create("John Carter"), Actor.Create("Elena Voss") }, AgeRating.Create(16), PosterUrl.Create("https://example.com/posters/shadow-protocol.jpg"), EntityId.New<MovieId>(Guid.Parse("c1f4c9d1-7b6a-4f5e-9f3b-2a7d9d8c1a01"))),
            Movie.Create("Echoes of Tomorrow", "Time travelers try to fix a broken future.", ReleaseYear.Create(2023), Duration.Create(130), new List<Genre> { Genre.Create("Sci-Fi"), Genre.Create("Drama") }, new List<Actor> { Actor.Create("Mila Hart"), Actor.Create("Jason Reed") }, AgeRating.Create(12), PosterUrl.Create("https://example.com/posters/echoes.jpg"), EntityId.New<MovieId>(Guid.Parse("a8b2e7c3-1d44-4f91-9b0f-6c0f2b9a7c22"))),
            Movie.Create("Crimson Night", "A detective hunts a serial killer in a dark city.", ReleaseYear.Create(2021), Duration.Create(105), new List<Genre> { Genre.Create("Crime"), Genre.Create("Mystery") }, new List<Actor> { Actor.Create("Liam Stone"), Actor.Create("Rachel Grey") }, AgeRating.Create(18), PosterUrl.Create("https://example.com/posters/crimson-night.jpg"), EntityId.New<MovieId>(Guid.Parse("f3d91b75-5d3a-4cbb-8c4f-1d2a1b7c8e33"))),
            Movie.Create("Ocean's Secret", "Divers uncover an ancient underwater civilization.", ReleaseYear.Create(2020), Duration.Create(112), new List<Genre> { Genre.Create("Adventure"), Genre.Create("Fantasy") }, new List<Actor> { Actor.Create("Chris Bay"), Actor.Create("Nina Shore") }, AgeRating.Create(10), PosterUrl.Create("https://example.com/posters/ocean-secret.jpg"), EntityId.New<MovieId>(Guid.Parse("9a7f2d6c-0e5c-4b2f-9c55-3a1b7c9e4f44"))),
            Movie.Create("Steel Horizon", "A war between humans and AI reaches its peak.", ReleaseYear.Create(2024), Duration.Create(140), new List<Genre> { Genre.Create("Action"), Genre.Create("Sci-Fi") }, new List<Actor> { Actor.Create("Victor Kane"), Actor.Create("Sara Lin") }, AgeRating.Create(16), PosterUrl.Create("https://example.com/posters/steel-horizon.jpg"), EntityId.New<MovieId>(Guid.Parse("bb4e7c81-6c9d-4f4d-bdb3-0d5f2a9e8c55"))),
            Movie.Create("Golden Fields", "A heartfelt story about family and legacy.", ReleaseYear.Create(2019), Duration.Create(95), new List<Genre> { Genre.Create("Drama") }, new List<Actor> { Actor.Create("Emma Clarke"), Actor.Create("Daniel Frost") }, AgeRating.Create(6), PosterUrl.Create("https://example.com/posters/golden-fields.jpg"), EntityId.New<MovieId>(Guid.Parse("de2f3c49-8f2b-4e0c-8d9a-4c1f2e7b6d66"))),
            Movie.Create("Neon Streets", "A cyberpunk city hides secrets in every alley.", ReleaseYear.Create(2025), Duration.Create(125), new List<Genre> { Genre.Create("Sci-Fi"), Genre.Create("Crime") }, new List<Actor> { Actor.Create("Axel Ray"), Actor.Create("Luna Vega") }, AgeRating.Create(16), PosterUrl.Create("https://example.com/posters/neon-streets.jpg"), EntityId.New<MovieId>(Guid.Parse("71c5e6a2-2d1f-4c7e-a0d4-9e2a7b3f8a77"))),
            Movie.Create("Frozen Peak", "Climbers face deadly conditions on a frozen mountain.", ReleaseYear.Create(2018), Duration.Create(102), new List<Genre> { Genre.Create("Adventure"), Genre.Create("Thriller") }, new List<Actor> { Actor.Create("Mark Ice"), Actor.Create("Sophie Hill") }, AgeRating.Create(12), PosterUrl.Create("https://example.com/posters/frozen-peak.jpg"), EntityId.New<MovieId>(Guid.Parse("8c1b4e2f-3a9d-4d2b-bf6c-5f7a9d2e9b88"))),
            Movie.Create("Laugh Track", "A struggling comedian finds unexpected fame.", ReleaseYear.Create(2021), Duration.Create(98), new List<Genre> { Genre.Create("Comedy") }, new List<Actor> { Actor.Create("Tom Bright"), Actor.Create("Ella Joy") }, AgeRating.Create(10), PosterUrl.Create("https://example.com/posters/laugh-track.jpg"), EntityId.New<MovieId>(Guid.Parse("5d7e2a91-0c4b-4f6d-9b8a-6c2e1f7d3c99")))
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
