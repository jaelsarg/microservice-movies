using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record Duration(int Value) : ValueObject
{
    public static Duration Create(int value)
    {
        EnsureDurationIsValid(value);
        return new Duration(value);
    }

    private static void EnsureDurationIsValid(int duration)
    {
        if (duration <= 0) throw new ArgumentException("Duration must be a positive integer.");
    }
}