using System.Linq.Expressions;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Queries;

public sealed class SearchMovieCatalogQuery(
    QueryDbContext context
) : ISearchMovieCatalogQuery
{
    public async Task<IReadOnlyList<MovieData>> Fetch(Expression<Func<MovieData, bool>> filter)
    {
        List<MovieData> movies = await context.Movies
            .Select(movie => new MovieData
            {
                Id = movie.Id,
                PosterUrl = movie.PosterUrl,
                Title = movie.Title,
                Genres = movie.Genres.Select(genre => new GenreData { Value = genre.Value }).ToList(),
                Actors = movie.Actors.Select(actor => new ActorData { Value = actor.Value }).ToList(),
                AgeRating = movie.AgeRating,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Description = movie.Description
            })
            .ToListAsync();

        return movies.Where(filter.Compile()).ToList();
    }
}