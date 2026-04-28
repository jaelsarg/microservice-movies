using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Shared.DomainEvents;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application.Mocks;

public sealed class InMemoryUnitOfWork(
    IDomainEventPublisher domainEventPublisher,
    ILogger<InMemoryUnitOfWork> logger
) : IUnitOfWork
{
    private readonly List<IAggregateRoot> _aggregates = [];
    private readonly ILogger<InMemoryUnitOfWork> _logger = logger;
    private readonly IDomainEventPublisher _domainEventPublisher = domainEventPublisher;
    private readonly Dictionary<string, object> _repositoriesWithRepoKey = [];

    public ILogger<InMemoryUnitOfWork> Logger => _logger;

    public async Task Do()
    {
        IReadOnlyList<IDomainEvent> domainEvents = [.. _aggregates
            .Where(x => x.DomainEvents.Count != 0)
            .SelectMany(x => 
            {
                var domainEvents = x.DomainEvents.ToList();
                x.ClearDomainEvents();
                return domainEvents;
            })];

        foreach (var domainEvent in domainEvents)
            await _domainEventPublisher.Publish(domainEvent);
    }

    public TRepository Repo<TRepository>()
        where TRepository : IRepository
    {
        string repoKey = typeof(TRepository).FullName
            ?? throw new InvalidOperationException("Cannot get full name of type.");

        if (_repositoriesWithRepoKey.TryGetValue(repoKey, out object? value))
            return (TRepository)value;

        throw new InvalidOperationException($"Repository for type {repoKey} not found.");
    }

    public async Task Save<TRepository>(IAggregateRoot aggregate)
        where TRepository : IRepository
    {
        string repoKey = typeof(TRepository).FullName
            ?? throw new InvalidOperationException("Cannot get full name of type.");

        if (!_repositoriesWithRepoKey.TryGetValue(repoKey, out object? repositoryObj))
            throw new InvalidOperationException($"Repository for type {repoKey} not found.");

        await ((dynamic)repositoryObj).Save((dynamic)aggregate);
        
        TrackAggregate(aggregate);
    }

    public void RegisterRepository<TRepository>(
        IRepository repository
    )
        where TRepository : IRepository
    {
        string repoKey = typeof(TRepository).FullName
            ?? throw new InvalidOperationException("Cannot get full name of type.");

        _repositoriesWithRepoKey[repoKey] = repository;
    }

    private void TrackAggregate(IAggregateRoot aggregate)
    {
        _aggregates.Add(aggregate);
    }
}
