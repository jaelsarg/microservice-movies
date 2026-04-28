namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record MovieData
{
    public Guid Id { get; init; }
    public string PosterUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public IReadOnlyList<GenreData> Genres { get; init; } = new List<GenreData>();
    public IReadOnlyList<ActorData> Actors { get; init; } = new List<ActorData>();
    public int AgeRating { get; init; }
    public int ReleaseYear { get; init; }
    public int Duration { get; init; }
    public string Description { get; init; } = string.Empty;
}