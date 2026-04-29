using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Data;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration;

public abstract class QueryDbContext : DbContext
{
    public DbSet<MovieData> Movies { get; set; }
    public DbSet<RoomData> Rooms { get; set; }
    public DbSet<MovieEventData> MovieEvents { get; set; }
    
    protected QueryDbContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new MovieDataConfiguration())
            .ApplyConfiguration(new RoomDataConfiguration())
            .ApplyConfiguration(new MovieEventDataConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}