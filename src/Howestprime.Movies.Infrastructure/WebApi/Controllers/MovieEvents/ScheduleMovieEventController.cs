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
    [Required] Guid MovieId,
    [Required] DateTime RequestedShowtime,
    [Required] Guid RoomId
);

public static class ScheduleMovieEventController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [AsParameters] ScheduleMovieEventRequest request
    )
    {
        ScheduleMovieEventInput input = new(
            request.Body.MovieId,
            request.Body.RequestedShowtime,
            request.Body.RoomId
        );

        Guid movieEventId = await request.UseCase.Execute(input);

        return TypedResults.Created($"/api/movie-events/{movieEventId}");
    }
}
