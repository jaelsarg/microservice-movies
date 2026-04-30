using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.MovieEvents;

public record SearchMovieEventsInTimerangeInput(
    DateTime FromDate,
    DateTime ToDate
);

public sealed class SearchMovieEventsInTimerange(
    IMovieEventQuery movieEventQuery
) : IUseCase<SearchMovieEventsInTimerangeInput, IReadOnlyList<MovieEventData>>
{
    public Task<IReadOnlyList<MovieEventData>> Execute(SearchMovieEventsInTimerangeInput input)
    {
        return movieEventQuery.Fetch(
            MovieEventDataFilters.ByTimeRange(input.FromDate, input.ToDate)
        );
    }
}