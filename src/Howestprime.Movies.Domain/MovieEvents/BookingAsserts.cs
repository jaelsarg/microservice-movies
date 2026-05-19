namespace Howestprime.Movies.Domain.MovieEvents;

public static class BookingAsserts
{
    public static void EnsureVisitorsAreGreaterThanZero(int standardVisitors, int discountedVisitors)
    {
        if (standardVisitors + discountedVisitors <= 0)
            throw new ArgumentException("The total number of visitors must be greater than 0.");
    }

    public static void EnsureSeatNumbersMatchVisitors(List<int> seatNumbers, int standardVisitors, int discountedVisitors)
    {
        if (seatNumbers.Count != standardVisitors + discountedVisitors)
            throw new ArgumentException("The number of seat numbers must match the total number of visitors.");
    }
}