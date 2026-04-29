using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record SearchMovieCatalogRequest(
    [FromQuery] string Title,
    [FromQuery] string Genres,
    [FromHeader(Name = "x-user-role")] string UserRole,
    [FromServices] IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>> UseCase
);

public record SearchMovieCatalogResponse(
    IReadOnlyList<MovieData> Movies
);

public static class SearchMovieCatalogController
{
    public static async Task<Results<Ok<SearchMovieCatalogResponse>, BadRequest<string>>> Invoke(
        [AsParameters] SearchMovieCatalogRequest request
    )
    {
        if (request.UserRole.ToLower() != "manager")
        {
            return TypedResults.BadRequest("Access denied");
        }

        SearchMovieCatalogInput input = new(
            request.Title,
            request.Genres.Split(',').Select(genre => genre.Trim()).ToList()
        );

        IReadOnlyList<MovieData> movies = await request.UseCase.Execute(input);

        return TypedResults.Ok(new SearchMovieCatalogResponse(movies));
    }
}