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
    [Required] string Title,
    [Required] string Description,
    [Required] int Duration,
    [Required] List<string> Genres,
    [Required] int ReleaseYear,
    [Required] List<string> Actors,
    [Required] int AgeRating,
    [Required] string PosterUrl
);

public static class RegisterMovieController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [AsParameters] RegisterMovieRequest request
    )
    {
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