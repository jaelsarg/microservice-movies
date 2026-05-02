using Aornis;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record FindMovieByIdRequest(
    [FromHeader(Name = "x-user-role")] string UserRole,
    [FromRoute] string Id,
    [FromServices] IUseCase<FindMovieByIdInput, Optional<MovieData>> UseCase
);

public static class FindMovieByIdController
{
    public static async Task<Results<Ok<Movie>, NotFound, BadRequest<string>>> Invoke(
        [AsParameters] FindMovieByIdRequest request
    )
    {
        if (request.UserRole.ToLower() != "manager")
        {
            return TypedResults.BadRequest("Access denied.");
        }

        FindMovieByIdInput input = new(Guid.Parse(request.Id));

        MovieData movieData = (await request.UseCase.Execute(input))
            .OrElseThrow(() => new NotFoundException($"Movie with id {request.Id} not found."));
        
        return TypedResults.Ok(BuildResponse(movieData));
    }

    private static Movie BuildResponse(MovieData movieData)
    {
        return new Movie(
            movieData.Id,
            movieData.Title,
            movieData.Description,
            movieData.ReleaseYear,
            movieData.Genres.Select(g => g.Value).ToList(),
            movieData.Actors.Select(a => a.Value).ToList(),
            movieData.AgeRating,
            movieData.PosterUrl
        );
    }
}