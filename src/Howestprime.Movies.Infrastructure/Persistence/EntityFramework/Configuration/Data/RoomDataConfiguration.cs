using Howestprime.Movies.Application.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Data;

public sealed class RoomDataConfiguration : IEntityTypeConfiguration<RoomData>
{
    public void Configure(EntityTypeBuilder<RoomData> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(room => room.Id);

        builder.Property(room => room.Name);
        builder.Property(room => room.Capacity);
    }
}