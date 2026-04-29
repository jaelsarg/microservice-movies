using Howestprime.Movies.Domain.MovieEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Domain;

public class MovieEventConfiguration : IEntityTypeConfiguration<MovieEvent>
{
    public void Configure(EntityTypeBuilder<MovieEvent> builder)
    {
        builder.ToTable("MovieEvents");

        builder.HasKey(movieEvent => movieEvent.Id);
        builder.Property(movieEvent => movieEvent.Id).ValueGeneratedNever();

        builder.Property(movieEvent => movieEvent.MovieId).IsRequired();
        builder.Property(movieEvent => movieEvent.RoomId).IsRequired();
        builder.Property(movieEvent => movieEvent.Showtime).IsRequired();
        builder.Property(movieEvent => movieEvent.Capacity).IsRequired();
    }
}