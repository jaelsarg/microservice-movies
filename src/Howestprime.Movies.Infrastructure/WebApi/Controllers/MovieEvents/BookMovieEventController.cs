using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public record BookMovieEventRequest(
    [FromBody] BookMovieEventBody Body,
    [FromRoute] Guid MovieEventId,
    [FromServices] IUseCase<BookMovieEventInput, Guid> UseCase
);

public record BookMovieEventBody(
    int StandardVisitors,
    int DiscountedVisitors
);

public static class BookMovieEventController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [AsParameters] BookMovieEventRequest request
    )
    {
        Console.WriteLine($"Received request to book movie event {request.MovieEventId} for {request.Body.StandardVisitors} standard visitors and {request.Body.DiscountedVisitors} discounted visitors.");

        BookMovieEventInput input = new(
            request.MovieEventId,
            request.Body.StandardVisitors,
            request.Body.DiscountedVisitors
        );

        Guid bookingId = await request.UseCase.Execute(input);

        return TypedResults.Created($"/api/movie-events/{request.MovieEventId}/bookings/{bookingId}");
    }
}