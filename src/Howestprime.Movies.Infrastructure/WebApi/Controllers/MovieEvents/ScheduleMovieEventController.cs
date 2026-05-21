using System.ComponentModel.DataAnnotations;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;

public record ScheduleMovieEventRequest(
    [FromBody] ScheduleMovieEventBody Body,
    [FromServices] IUseCase<ScheduleMovieEventInput, Guid> UseCase
);

public record ScheduleMovieEventBody(
    Guid MovieId,
    Guid RoomId,
    DateTime Showtime
);

public static class ScheduleMovieEventController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [AsParameters] ScheduleMovieEventRequest request
    )
    {
        Console.WriteLine($"Received request to schedule movie event for movie {request.Body.MovieId} in room {request.Body.RoomId} at {request.Body.Showtime}");

        ScheduleMovieEventInput input = new(
            request.Body.MovieId,
            request.Body.Showtime,
            request.Body.RoomId
        );

        Guid movieEventId = await request.UseCase.Execute(input);

        return TypedResults.Created($"/api/movie-events/{movieEventId}");
    }
}
