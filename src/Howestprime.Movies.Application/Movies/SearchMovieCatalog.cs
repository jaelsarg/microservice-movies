using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Data.Filters;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies;

public record SearchMovieCatalogInput(
    string? Title,
    List<string>? Genres
);

public sealed class SearchMovieCatalog(
    ISearchMovieCatalogQuery searchMovieCatalogQuery
) : IUseCase<SearchMovieCatalogInput, IReadOnlyList<MovieData>>
{
    public Task<IReadOnlyList<MovieData>> Execute(SearchMovieCatalogInput input)
    {
        IReadOnlyList<GenreData> normalizedGenres = input.Genres?.Select(genre => new GenreData { Value = genre.ToLower() }).ToList() ?? new List<GenreData>();
        string normalizedTitle = input.Title?.ToLower() ?? string.Empty;

        return searchMovieCatalogQuery.Fetch(
            MovieDataFilters.ByTitleAndGenres(normalizedTitle, normalizedGenres)
        );
    }
}