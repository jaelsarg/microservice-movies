namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Responses;

public sealed record MovieCollection(
    IReadOnlyList<Movie> Data
);