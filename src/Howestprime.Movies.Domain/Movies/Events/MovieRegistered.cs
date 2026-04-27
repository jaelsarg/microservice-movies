using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieRegistered (
    MovieId id,
    string title,
    ReleaseYear releaseYear,
    Duration duration,
    List<Genre> genres,
    List<Actor> actors,
    AgeRating ageRating,
    PosterUrl posterUrl
) : MovieDomainEvent(nameof(MovieRegistered))
{
    public MovieId MovieId { get; private init; } = id;
    public string Title { get; private init; } = title;
    public ReleaseYear ReleaseYear { get; private init; } = releaseYear;
    public Duration Duration { get; private init; } = duration;
    public List<Genre> Genres { get; private init; } = genres;
    public List<Actor> Actors { get; private init; } = actors;
    public AgeRating AgeRating { get; private init; } = ageRating;
    public PosterUrl PosterUrl { get; private init; } = posterUrl;
}