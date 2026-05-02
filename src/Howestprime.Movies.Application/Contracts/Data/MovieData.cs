namespace Howestprime.Movies.Application.Contracts.Data;

public sealed record MovieData
{
    public Guid Id { get; init; }
    public string PosterUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public IReadOnlyList<GenreData> Genres { get; init; } = new List<GenreData>();
    public IReadOnlyList<ActorData> Actors { get; init; } = new List<ActorData>();
    public int AgeRating { get; init; } = 0;
    public int ReleaseYear { get; init; } = 0;
    public int Duration { get; init; } = 0;
    public string Description { get; init; } = string.Empty;
}