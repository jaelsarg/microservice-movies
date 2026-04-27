using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record Genre(string Value) : ValueObject
{
    public static Genre Create(string value)
    {
        EnsureGenreIsValid(value);
        return new Genre(value);
    }

    private static void EnsureGenreIsValid(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("Genre cannot be empty.");
    }
}