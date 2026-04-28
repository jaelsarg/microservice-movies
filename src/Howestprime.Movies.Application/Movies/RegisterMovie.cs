using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
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
            input.ReleaseYear,
            input.Duration,
            input.Genres,
            input.Actors,
            input.AgeRating,
            input.PosterUrl
        );

        await _uow.Save<IMovieRepository>(movie);

        await _uow.Do();

        _logger.LogInformation("Movie with id {movie.Id} registered", movie.Id);

        return movie.Id.ToString();
    }
}