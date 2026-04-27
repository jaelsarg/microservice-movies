using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Howestprime.Movies.Domain.Movies.Events;

namespace Howestprime.Movies.Domain.Movies;

public readonly record struct MovieId(Guid Value) : IEntityId;

public sealed class Movie : AggregateRoot<MovieId>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public ReleaseYear ReleaseYear { get; private set; }
    public Duration Duration { get; private set; }
    public List<Genre> Genres { get; private set; }
    public List<Actor> Actors { get; private set; }
    public AgeRating AgeRating { get; private set; }
    public PosterUrl PosterUrl { get; private set; }

    private Movie(
        MovieId id,
        string title,
        string description,
        ReleaseYear releaseYear,
        Duration duration,
        List<Genre> genres,
        List<Actor> actors,
        AgeRating ageRating,
        PosterUrl posterUrl
    ) : base(id)
    {
        Title = title;
        Description = description;
        ReleaseYear = releaseYear;
        Duration = duration;
        Genres = genres;
        Actors = actors;
        AgeRating = ageRating;
        PosterUrl = posterUrl;
    }

    public static Movie Create(
        string title,
        string description,
        int releaseYear,
        int duration,
        List<string> genres,
        List<string> actors,
        int ageRating,
        string posterUrl
    )
    {
        Movie movie = new(
            EntityId.New<MovieId>(),
            title,
            description,
            ReleaseYear.Create(releaseYear),
            Duration.Create(duration),
            genres.Select(Genre.Create).ToList(),
            actors.Select(Actor.Create).ToList(),
            AgeRating.Create(ageRating),
            PosterUrl.Create(posterUrl)
        );

        movie.ValidateState();

        movie.RaiseDomainEvent(
            new MovieRegistered(
                movie.Id,
                movie.Title,
                movie.ReleaseYear,
                movie.Duration,
                movie.Genres,
                movie.Actors,
                movie.AgeRating,
                movie.PosterUrl
            )
        );

        return movie;
    }

    public override void ValidateState()
    {
        Asserts.EnsureNotEmpty(Title);
        Asserts.EnsureNotEmpty(Description);
    }
}
