namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record PosterUrl(string Value)
{
    public static PosterUrl Create(string value)
    {
        EnsurePosterUrlIsValid(value);
        return new PosterUrl(value);
    }

    private static void EnsurePosterUrlIsValid(string posterUrl)
    {
        if (string.IsNullOrWhiteSpace(posterUrl)) throw new ArgumentException("Poster URL cannot be empty.");
    }
}