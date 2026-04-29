using System.Linq.Expressions;

namespace Howestprime.Movies.Application.Contracts.Data.Filters;

public static class MovieDataFilters
{
    public static Expression<Func<MovieData, bool>> ByTitleAndGenres(string title, IReadOnlyList<GenreData> genres)
    {
        string normalizedTitle = title.ToLower();
        string[] normalizedGenresValues = genres.Select(genre => genre.Value.ToLower()).ToArray();
        
        return movie => movie.Title.ToLower().Contains(normalizedTitle)
            && normalizedGenresValues.All(genre => movie.Genres.Any(movieGenre => movieGenre.Value.ToLower().Contains(genre)));
    }
}