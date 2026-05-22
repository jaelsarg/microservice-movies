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

        builder.OwnsMany(movieEvent => movieEvent.Bookings, bookingBuilder =>
        {
            bookingBuilder.ToJson();
            
            bookingBuilder.Property(booking => booking.Id).ValueGeneratedNever();

            bookingBuilder.Property(booking => booking.BookingStatus)
                .HasConversion<string>()
                .IsRequired();

            bookingBuilder.Property(booking => booking.PaymentStatus)
                .HasConversion<string>()
                .IsRequired();

            bookingBuilder.Property(booking => booking.StandardVisitors).IsRequired();
            bookingBuilder.Property(booking => booking.DiscountVisitors).IsRequired();
            bookingBuilder.Property(booking => booking.SeatNumbers).IsRequired();
            
        });

        builder.Property(movieEvent => movieEvent.Visitors).IsRequired();
    }
}