namespace Howestprime.Movies.Domain.Movies;

public static class MovieAsserts
{
    public static void EnsureReleaseYearIsInPast(int releaseYear)
    {
        if (releaseYear > DateTime.Now.Year)
            throw new ArgumentException($"Release year {releaseYear} must be in the past.", nameof(releaseYear));
    }
}