using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record ChangeMovieDetailsRequest(
    [FromRoute] string Id,
    [FromBody] ChangeMovieDetailsBody Body,
    [FromServices] IUseCase<ChangeMovieDetailsInput> UseCase
);
public record ChangeMovieDetailsBody(
    [Required] string Title,
    [Required] string Description,
    [Required] int Duration,
    [Required] List<string> Genres,
    [Required] int ReleaseYear,
    [Required] List<string> Actors,
    [Required] int AgeRating,
    [Required] string PosterUrl
);

public static class ChangeMovieDetailsController
{
    public static async Task<Results<NoContent, BadRequest>> Invoke(
        [AsParameters] ChangeMovieDetailsRequest request
    )
    {
        ChangeMovieDetailsInput input = new(
            Guid.Parse(request.Id),
            request.Body.Title,
            request.Body.Description,
            request.Body.Duration,
            request.Body.Genres,
            request.Body.ReleaseYear,
            request.Body.Actors,
            request.Body.AgeRating,
            request.Body.PosterUrl
        );

        await request.UseCase.Execute(input);
        
        return TypedResults.NoContent();
    }
}