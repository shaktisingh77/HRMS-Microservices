using System.Text.Json;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Application.Outbox;
using HRMS.Employee.Infrastructure.Persistence;

namespace HRMS.Employee.Infrastructure.Outbox;

public sealed class OutboxWriter : IOutboxWriter
{
    private readonly EmployeeDbContext _dbContext;

    public OutboxWriter(EmployeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(IIntegrationEvent integrationEvent)
    {
        var message = new OutboxMessage(Guid.NewGuid(), integrationEvent.GetType().AssemblyQualifiedName!,
                                        JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
                                        DateTime.UtcNow);

        await _dbContext.OutboxMessages.AddAsync(message);
    }
}