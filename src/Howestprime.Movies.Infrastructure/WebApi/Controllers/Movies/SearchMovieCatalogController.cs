using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record SearchMovieCatalogRequest(
    [FromQuery] string? Title,
    [FromQuery] string? Genres,
    [FromHeader(Name = "x-user-role")] string UserRole,
    [FromServices] IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>> UseCase
);

public static class SearchMovieCatalogController
{
    public static async Task<Results<Ok<MovieCollection>, BadRequest<string>>> Invoke(
        [AsParameters] SearchMovieCatalogRequest request
    )
    {
        Console.WriteLine($"Received request to search movie catalog with title {request.Title} and genres {request.Genres} from user with role {request.UserRole}.");

        if (request.UserRole.ToLower() != "manager")
        {
            return TypedResults.BadRequest("Access denied");
        }

        SearchMovieCatalogInput input = new(
            request.Title,
            request.Genres?.Split(',').Select(genre => genre.Trim()).ToList()
        );

        IReadOnlyList<MovieData> movies = await request.UseCase.Execute(input);

        return TypedResults.Ok(BuildResponse(movies));
    }

    private static MovieCollection BuildResponse(IReadOnlyList<MovieData> movies)
    {
        return new MovieCollection(
            movies.Select(movie => new Movie(
                movie.Id,
                movie.Title,
                movie.Description,
                movie.ReleaseYear,
                movie.Duration,
                movie.Genres.Select(g => g.Value).ToList(),
                movie.Actors.Select(a => a.Value).ToList(),
                movie.AgeRating,
                movie.PosterUrl
            )).ToList()
        );
    }
}