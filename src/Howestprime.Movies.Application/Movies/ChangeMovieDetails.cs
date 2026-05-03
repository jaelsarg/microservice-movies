using Aornis;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Application.Movies;

public sealed record ChangeMovieDetailsInput(
    Guid Id,
    string Title,
    string Description,
    int Duration,
    List<string> Genres,
    int ReleaseYear,
    List<string> Actors,
    int AgeRating,
    string PosterUrl
);

public sealed class ChangeMovieDetails(
    IUnitOfWork _uow,
    ILogger<ChangeMovieDetails> _logger
) : IUseCase<ChangeMovieDetailsInput>
{
    public async Task Execute(ChangeMovieDetailsInput input)
    {
        MovieId movieId = new(input.Id);
        Optional<Movie> movieOptional = await _uow.Repo<IMovieRepository>().ById(movieId);

        if (!movieOptional.HasValue) throw new NotFoundException($"Movie with id {input.Id} not found.");
        Movie movie = movieOptional.Value;

        movie.ChangeDetails(
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

        _logger.LogInformation($"Movie with id {movie.Id} details changed");
    }
}