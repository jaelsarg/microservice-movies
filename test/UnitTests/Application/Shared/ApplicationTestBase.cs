using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.MovieEvents;
using Howestprime.Movies.Application.Movies;
using Howestprime.Movies.Domain.MovieEvents;
using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Rooms;
using Howestprime.Movies.Domain.Shared.DomainEvents;
using Howestprime.Movies.Infrastructure.Messaging.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnitTests.Application.Mocks;
using UnitTests.Mocks;

namespace UnitTests.Application.Shared;

public abstract class ApplicationTestBase
{
    protected readonly IDomainEventPublisher DomainEventPublisher;
    protected readonly InMemoryMovieRepository MovieRepository;
    protected readonly InMemoryMovieEventRepository MovieEventRepository;
    protected readonly InMemoryRoomRepository RoomRepository;
    protected readonly InMemoryUnitOfWork UnitOfWork;

    protected ApplicationTestBase()
    {
        DomainEventPublisher = new DomainEventPublisher([], CreateLogger<DomainEventPublisher>());
        MovieRepository = new InMemoryMovieRepository();
        MovieEventRepository = new InMemoryMovieEventRepository();
        RoomRepository = new InMemoryRoomRepository();

        UnitOfWork = new InMemoryUnitOfWork(DomainEventPublisher, CreateLogger<InMemoryUnitOfWork>());
        UnitOfWork.RegisterRepository<IMovieRepository>(MovieRepository);
        UnitOfWork.RegisterRepository<IMovieEventRepository>(MovieEventRepository);
        UnitOfWork.RegisterRepository<IRoomRepository>(RoomRepository);
    }

    protected static TestLogger<T> CreateLogger<T>() => new();
}

// Only for tests that require DI, e.g., to test policies.
public abstract class ApplicationTestBaseWithDI
{
    protected readonly IServiceProvider ServiceProvider;

    protected ApplicationTestBaseWithDI()
    {
        IServiceCollection services = new ServiceCollection();

        RegisterLogging(services);
        RegisterRepositories(services);
        RegisterUseCases(services);
        RegisterPolicies(services);
        RegisterDomainPublisher(services);
        RegisterUnitOfWork(services);

        ServiceProvider = services.BuildServiceProvider();
    }

    protected IUnitOfWork CreateUnitOfWork() =>
        ServiceProvider.GetRequiredService<IUnitOfWork>();

    private static void RegisterLogging(IServiceCollection services)
    {
        services.AddSingleton(typeof(ILogger<>), typeof(TestLogger<>));
    }

    // Watch out: they are registered as Singletons to keep data in-memory across UoW instances (testing only).
    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
        services.AddSingleton<IMovieEventRepository, InMemoryMovieEventRepository>();
        services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        services.AddScoped<IUseCase<RegisterMovieInput, string>, RegisterMovie>();
        services.AddScoped<IUseCase<ScheduleMovieEventInput, Guid>, ScheduleMovieEvent>();
    }

    private static void RegisterPolicies(IServiceCollection services)
    {
    }

    private static void RegisterUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(sp =>
        {
            IDomainEventPublisher publisher = sp.GetRequiredService<IDomainEventPublisher>();

            var uow = new InMemoryUnitOfWork(
                publisher,
                CreateLogger<InMemoryUnitOfWork>());

            uow.RegisterRepository<IMovieRepository>(
                sp.GetRequiredService<IMovieRepository>());

            return uow;
        });
    }

    private static void RegisterDomainPublisher(IServiceCollection services)
    {
        services.AddSingleton<IDomainEventPublisher>(sp =>
        {
            IList<IDomainEventListener> listeners = [
                new PolicyDomainEventListenerWithDI(sp, CreateLogger<PolicyDomainEventListenerWithDI>())
            ];

            return new DomainEventPublisher(
                listeners,
                CreateLogger<DomainEventPublisher>());
        });
    }

    protected static TestLogger<T> CreateLogger<T>() => new();
}