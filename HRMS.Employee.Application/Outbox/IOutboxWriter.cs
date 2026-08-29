using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Employee.Application.Outbox;

public interface IOutboxWriter
{
    Task AddAsync(IIntegrationEvent integrationEvent);
}