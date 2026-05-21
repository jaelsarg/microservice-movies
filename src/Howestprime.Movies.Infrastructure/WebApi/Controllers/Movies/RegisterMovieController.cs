using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;

public record RegisterMovieRequest(
    [FromBody] RegisterMovieBody Body,
    [FromServices] IUseCase<RegisterMovieInput, string> UseCase
);

public record RegisterMovieBody(
    string Title,
    string Description,
    int Duration,
    List<string> Genres,
    int ReleaseYear,
    List<string> Actors,
    int AgeRating,
    string PosterUrl
);

public static class RegisterMovieController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [AsParameters] RegisterMovieRequest request
    )
    {
        Console.WriteLine($"Received request to register movie with title {request.Body.Title}, description {request.Body.Description}, duration {request.Body.Duration}, genres {string.Join(", ", request.Body.Genres)}, release year {request.Body.ReleaseYear}, actors {string.Join(", ", request.Body.Actors)}, age rating {request.Body.AgeRating} and poster url {request.Body.PosterUrl}.");

        RegisterMovieInput input = new(
            request.Body.Title,
            request.Body.Description,
            request.Body.Duration,
            request.Body.Genres,
            request.Body.ReleaseYear,
            request.Body.Actors,
            request.Body.AgeRating,
            request.Body.PosterUrl
        );

        string movieId = await request.UseCase.Execute(input);
        
        return TypedResults.Created($"/api/movie-catalog/{movieId}");
    }
}