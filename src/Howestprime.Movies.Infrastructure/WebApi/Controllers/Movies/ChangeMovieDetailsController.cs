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
    string Title,
    string Description,
    int Duration,
    List<string> Genres,
    int ReleaseYear,
    List<string> Actors,
    int AgeRating,
    string PosterUrl
);

public static class ChangeMovieDetailsController
{
    public static async Task<Results<NoContent, BadRequest>> Invoke(
        [AsParameters] ChangeMovieDetailsRequest request
    )
    {
        Console.WriteLine($"Received request to change details of movie {request.Id} with title {request.Body.Title}, description {request.Body.Description}, duration {request.Body.Duration}, genres {string.Join(", ", request.Body.Genres)}, release year {request.Body.ReleaseYear}, actors {string.Join(", ", request.Body.Actors)}, age rating {request.Body.AgeRating} and poster url {request.Body.PosterUrl}.");

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