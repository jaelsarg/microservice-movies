using Aornis;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies;

public record FindMovieByIdInput(Guid Id);

public sealed class FindMovieById(
    ISearchMovieCatalogQuery searchMovieCatalogQuery
) : IUseCase<FindMovieByIdInput, Optional<MovieData>>
{
    public async Task<Optional<MovieData>> Execute(FindMovieByIdInput input)
    {
        IReadOnlyList<MovieData> movies = await searchMovieCatalogQuery.Fetch(MovieDataFilters.ById(input.Id));

        return movies.Any() ? movies.First() : Optional<MovieData>.Empty;
    }
}