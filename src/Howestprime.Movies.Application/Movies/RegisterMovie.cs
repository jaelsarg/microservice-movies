using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record RegisterMovieInput(
    string Title,
    string Description,
    int Duration,
    List<string> Genres,
    int ReleaseYear,
    List<string> Actors,
    int AgeRating,
    string PosterUrl
);

public sealed class RegisterMovie(
    IUnitOfWork _uow,
    ILogger<RegisterMovie> _logger
) : IUseCase<RegisterMovieInput, string>
{
    public async Task<string> Execute(RegisterMovieInput input)
    {
        Movie movie = Movie.Create(
            input.Title,
            input.Description,
            ReleaseYear.Create(input.ReleaseYear),
            Duration.Create(input.Duration),
            input.Genres.Select(Genre.Create).ToList(),
            input.Actors.Select(Actor.Create).ToList(),
            AgeRating.Create(input.AgeRating),
            PosterUrl.Create(input.PosterUrl)
        );

        await _uow.Save<IMovieRepository>(movie);

        await _uow.Do();

        _logger.LogInformation("Movie with id {movie.Id} registered", movie.Id);

        return movie.Id.ToString();
    }
}