using Howestprime.Movies.Application.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Data;

public sealed class MovieEventDataConfiguration : IEntityTypeConfiguration<MovieEventData>
{
    public void Configure(EntityTypeBuilder<MovieEventData> builder)
    {
        builder.ToTable("MovieEvents");
        builder.HasKey(movieEvent => movieEvent.Id);

        builder.Property(movieEvent => movieEvent.Showtime);
        builder.Property(movieEvent => movieEvent.Capacity);

        builder.HasOne(movieEvent => movieEvent.Room)
            .WithMany()
            .HasForeignKey("RoomId");

        builder.HasOne(movieEvent => movieEvent.Movie)
            .WithMany()
            .HasForeignKey("MovieId");
    }
}