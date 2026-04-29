using System.Linq.Expressions;
using Aornis;
using Howestprime.Movies.Domain.Movies;

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

    public static Expression<Func<MovieData, bool>> ById(Guid id)
    {
        return movie => movie.Id == id;
    }
}