using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record ReleaseYear(int Value) : ValueObject
{
    public static ReleaseYear Create(int value)
    {
        EnsureYearIsValid(value);
        return new ReleaseYear(value);
    }

    private static void EnsureYearIsValid(int year)
    {
        if (year < 0) throw new ArgumentException("Release year cannot be negative.");
    }
}