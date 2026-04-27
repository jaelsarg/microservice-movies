namespace Howestprime.Movies.Domain.Movies.ValueObjects;

public sealed record AgeRating(int Value)
{
    public static AgeRating Create(int value)
    {
        EnsureAgeRatingIsValid(value);
        return new AgeRating(value);
    }

    private static void EnsureAgeRatingIsValid(int ageRating)
    {
        if (ageRating < 0) throw new ArgumentException("Age rating cannot be negative.");
    }
}