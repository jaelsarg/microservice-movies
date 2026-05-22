namespace Howestprime.Movies.Domain.MovieEvents;

public static class BookingAsserts
{
    public static void EnsureVisitorsAreGreaterThanZero(int standardVisitors, int discountVisitors)
    {
        if (standardVisitors + discountVisitors <= 0)
            throw new ArgumentException("The total number of visitors must be greater than 0.");
    }

    public static void EnsureSeatNumbersMatchVisitors(List<int> seatNumbers, int standardVisitors, int discountVisitors)
    {
        if (seatNumbers.Count != standardVisitors + discountVisitors)
            throw new ArgumentException("The number of seat numbers must match the total number of visitors.");
    }
}