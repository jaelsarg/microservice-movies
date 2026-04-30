using Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;
using Howestprime.Movies.Infrastructure.WebApi.Controllers.Movies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Howestprime.Movies.Infrastructure.WebApi;

public static class Routes
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder webApi = app.MapGroup("/api");

        webApi.MapMovieRoutes();
        webApi.MapMovieEventRoutes();
        webApi.MapHowestprimeScheduleRoutes();

        return app;
    }

    private static RouteGroupBuilder MapMovieRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder movies = app.MapGroup("/movie-catalog")
            .WithTags("Movie, Movies, Movie Catalog")
            .WithDescription("All endpoints related to managing the movie catalog.");

        movies.MapPost("", RegisterMovieController.Invoke)
            .WithName("RegisterMovie")
            .WithDescription("Register a new movie in the catalog.");
        
        movies.MapGet("", SearchMovieCatalogController.Invoke)
            .WithName("SearchMovieCatalog")
            .WithDescription("Search the movie catalog by title and/or genres.");
        
        movies.MapGet("/{Id}", FindMovieByIdController.Invoke)
            .WithName("FindMovieById")
            .WithDescription("Find a movie in the catalog by its ID.");

        return movies;
    }

    private static RouteGroupBuilder MapMovieEventRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder movieEvents = app.MapGroup("/movie-events")
            .WithTags("MovieEvent, MovieEvents, Movie Showtimes")
            .WithDescription("All endpoints related to managing movie events.");
        
        movieEvents.MapPost("", ScheduleMovieEventController.Invoke)
            .WithName("ScheduleMovieEvent")
            .WithDescription("Schedule a new movie event (showtime).");
        
        return movieEvents;
    }

    private static RouteGroupBuilder MapHowestprimeScheduleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder howestprimeSchedule = app.MapGroup("/howestprime-schedule")
            .WithTags("HowestprimeSchedule, Schedule, Movie Showtimes")
            .WithDescription("Endpoints related to the Howestprime schedule.");

        howestprimeSchedule.MapGet("", GetHowestprimeScheduleController.Invoke)
            .WithName("GetHowestprimeSchedule")
            .WithDescription("Get the schedule of movie events for the next 14 days.");

        return howestprimeSchedule;
    }
}