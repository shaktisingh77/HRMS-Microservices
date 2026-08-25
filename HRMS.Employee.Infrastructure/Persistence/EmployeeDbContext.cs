using Microsoft.EntityFrameworkCore;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;
using HRMS.Employee.Domain.DomainEvents;
using HRMS.Employee.Application.DomainEvents;

namespace HRMS.Employee.Infrastructure.Persistence;

public class EmployeeDbContext : DbContext
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options,
                             IDomainEventDispatcher domainEventDispatcher) : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker
                                    .Entries<IHasDomainEvents>()
                                    .Where(e => e.Entity.DomainEvents.Any())
                                    .Select(e => e.Entity)
                                    .ToList();

        var domainEvents = entitiesWithEvents
                                .SelectMany(e => e.DomainEvents)
                                .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _domainEventDispatcher.DispatchAsync(domainEvent);
        }

        foreach (var entity in entitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }

        return result;
    }
}