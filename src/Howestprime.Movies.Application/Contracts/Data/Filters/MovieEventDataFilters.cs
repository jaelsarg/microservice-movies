using System.Linq.Expressions;

namespace Howestprime.Movies.Application.Contracts.Data.Filters;

public static class MovieEventDataFilters
{
    public static Expression<Func<MovieEventData, bool>> ByTimeRange(DateTime fromDate, DateTime toDate)
    {
        DateTime utcFromDate = fromDate.ToUniversalTime();
        DateTime utcToDate = toDate.ToUniversalTime();

        return movieEvent => movieEvent.Showtime >= utcFromDate && movieEvent.Showtime <= utcToDate;
    }
}