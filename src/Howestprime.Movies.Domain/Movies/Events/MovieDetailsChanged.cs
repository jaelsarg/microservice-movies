using Howestprime.Movies.Domain.Movies.ValueObjects;

namespace Howestprime.Movies.Domain.Movies.Events;

public sealed class MovieDetailsChanged(
    MovieId id,
    string title,
    PosterUrl posterUrl,
    ReleaseYear releaseYear,
    Duration duration,
    List<Genre> genres,
    List<Actor> actors,
    AgeRating ageRating
) : MovieDomainEvent(nameof(MovieDetailsChanged))
{
    public MovieId MovieId { get; private init; } = id;
    public string Title { get; private init; } = title;
    public PosterUrl PosterUrl { get; private init; } = posterUrl;
    public ReleaseYear ReleaseYear { get; private init; } = releaseYear;
    public Duration Duration { get; private init; } = duration;
    public List<Genre> Genres { get; private init; } = genres;
    public List<Actor> Actors { get; private init; } = actors;
    public AgeRating AgeRating { get; private init; } = ageRating;
}