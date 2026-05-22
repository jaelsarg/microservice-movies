using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;

public record GetMovieEventsByMonthRequest(
    [FromQuery] int Year,
    [FromQuery] int Month,
    [FromServices] IUseCase<SearchMovieEventsInTimerangeInput, IReadOnlyList<MovieEventData>> UseCase
);

public static class GetMovieEventsByMonthController
{
    public static async Task<Ok<IReadOnlyList<MovieEvent>>> Invoke(
        [AsParameters] GetMovieEventsByMonthRequest request
    )
    {
        Console.WriteLine($"Received request to get movie events for year {request.Year} and month {request.Month}.");

        int year = request.Year;
        int month = request.Month;

        SearchMovieEventsInTimerangeInput input = new(
            new DateTime(year, month, 1),
            new DateTime(year, month, DateTime.DaysInMonth(year, month))
        );

        IReadOnlyList<MovieEventData> movieEvents = await request.UseCase.Execute(input);

        return TypedResults.Ok(BuildResponse(movieEvents));
    }

    private static IReadOnlyList<MovieEvent> BuildResponse(IReadOnlyList<MovieEventData> movieEvents)
    {
        return [.. movieEvents.Select(movieEvent => new MovieEvent(
                movieEvent.Id,
                new Movie(
                    movieEvent.Movie.Id,
                    movieEvent.Movie.Title,
                    movieEvent.Movie.Description,
                    movieEvent.Movie.ReleaseYear,
                    movieEvent.Movie.Duration,
                    movieEvent.Movie.Genres.Select(genre => genre.Value).ToList(),
                    movieEvent.Movie.Actors.Select(actor => actor.Value).ToList(),
                    movieEvent.Movie.AgeRating,
                    movieEvent.Movie.PosterUrl
                ),
                new Room(
                    movieEvent.Room.Id,
                    movieEvent.Room.Name,
                    movieEvent.Room.Capacity
                ),
                movieEvent.Showtime,
                movieEvent.Capacity
            )).ToList()];
    }   
}