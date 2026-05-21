using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;

public static class GetHowestprimeScheduleController
{
    public static async Task<Ok<HowestprimeSchedule>> Invoke(
        [FromServices] IUseCase<SearchMovieEventsInTimerangeInput, IReadOnlyList<MovieEventData>> useCase
    )
    {
        Console.WriteLine("Received request to get Howestprime schedule.");

        DateTime fromDate = DateTime.UtcNow;
        DateTime toDate = fromDate.AddDays(14);

        SearchMovieEventsInTimerangeInput input = new(fromDate, toDate);

        IReadOnlyList<MovieEventData> movieEvents = await useCase.Execute(input);

        return TypedResults.Ok(BuildResponse(movieEvents));
    }

    private static HowestprimeSchedule BuildResponse(IReadOnlyList<MovieEventData> movieEvents)
    {
        List<Guid> movieIds = movieEvents
            .Select(movieEvent => movieEvent.Movie.Id)
            .Distinct()
            .ToList();

        return new HowestprimeSchedule(
            movieIds,
            movieEvents.Select(movieEvent => new MovieEvent(
                movieEvent.Id,
                new Movie(
                    movieEvent.Movie.Id,
                    movieEvent.Movie.Title,
                    movieEvent.Movie.Description,
                    movieEvent.Movie.ReleaseYear,
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
            )).ToList()
        );
    }
}