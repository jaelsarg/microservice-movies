using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Queries;

public sealed class MovieEventQuery(
    QueryDbContext context
) : IMovieEventQuery
{
    public async Task<IReadOnlyList<MovieEventData>> Fetch(Expression<Func<MovieEventData, bool>> filter)
    {
        List<MovieEventData> movieEvents = await context.MovieEvents
            .Select(movieEvent => new MovieEventData
            {
                Id = movieEvent.Id,
                Showtime = movieEvent.Showtime,
                Capacity = movieEvent.Capacity,
                Room = new RoomData
                {
                    Id = movieEvent.Room.Id,
                    Name = movieEvent.Room.Name,
                    Capacity = movieEvent.Room.Capacity
                },
                Movie = new MovieData
                {
                    Id = movieEvent.Movie.Id,
                    PosterUrl = movieEvent.Movie.PosterUrl,
                    Title = movieEvent.Movie.Title,
                    Genres = movieEvent.Movie.Genres.Select(genre => new GenreData { Value = genre.Value }).ToList(),
                    Actors = movieEvent.Movie.Actors.Select(actor => new ActorData { Value = actor.Value }).ToList(),
                    AgeRating = movieEvent.Movie.AgeRating,
                    ReleaseYear = movieEvent.Movie.ReleaseYear,
                    Duration = movieEvent.Movie.Duration,
                    Description = movieEvent.Movie.Description
                }
            })
            .ToListAsync();
        
        return movieEvents.Where(filter.Compile()).ToList();
    }
}