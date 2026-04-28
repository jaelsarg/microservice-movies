using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Domain;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(movie => movie.Id);
        builder.Property(movie => movie.Id).ValueGeneratedNever();

        builder.Property(movie => movie.Title).IsRequired();
        builder.Property(movie => movie.Description).IsRequired();
        builder.Property(movie => movie.ReleaseYear).IsRequired();
        builder.Property(movie => movie.Duration).IsRequired();

        builder.OwnsMany(movie => movie.Genres, genreBuilder =>
        {
            genreBuilder.ToJson();
            genreBuilder.Property(genre => genre.Value).IsRequired();
        });

        builder.OwnsMany(movie => movie.Actors, actorBuilder =>
        {
            actorBuilder.ToJson();
            actorBuilder.Property(actor => actor.Value).IsRequired();
        });

        builder.Property(movie => movie.AgeRating)
            .HasConversion(
                ageRating => ageRating.Value,
                value => AgeRating.Create(value)
            )
            .IsRequired();
        builder.Property(movie => movie.PosterUrl)
            .HasConversion(
                posterUrl => posterUrl.Value,
                value => PosterUrl.Create(value)
            )
            .IsRequired();
    }
}