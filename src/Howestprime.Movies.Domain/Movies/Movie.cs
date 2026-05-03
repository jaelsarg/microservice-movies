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

    #pragma warning disable CS8618
    private Movie() :base(default!) {}
    #pragma warning restore CS8618

    public static Movie Create(
        string title,
        string description,
        int releaseYear,
        int duration,
        List<string> genres,
        List<string> actors,
        int ageRating,
        string posterUrl,
        MovieId? id = null
    )
    {
        Movie movie = new(
            id ?? EntityId.New<MovieId>(),
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
        Asserts.EnsureNotEmpty(Genres);
        Asserts.EnsureNotEmpty(Actors);
        Asserts.EnsureNotNegative(Duration.Value);
        Asserts.EnsureNotNegative(AgeRating.Value);
        Asserts.EnsureNotNegative(ReleaseYear.Value);
        MovieAsserts.EnsureReleaseYearIsInPast(ReleaseYear.Value);
    }

    public void ChangeDetails(
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
        Title = title;
        Description = description;
        ReleaseYear = ReleaseYear.Create(releaseYear);
        Duration = Duration.Create(duration);
        Genres = genres.Select(Genre.Create).ToList();
        Actors = actors.Select(Actor.Create).ToList();
        AgeRating = AgeRating.Create(ageRating);
        PosterUrl = PosterUrl.Create(posterUrl);

        ValidateState();

        RaiseDomainEvent(
            new MovieDetailsChanged(
                Id,
                Title,
                PosterUrl,
                ReleaseYear,
                Duration,
                Genres,
                Actors,
                AgeRating
            )
        );
    }
}
