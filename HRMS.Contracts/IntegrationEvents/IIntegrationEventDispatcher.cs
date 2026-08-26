namespace HRMS.Contracts.IntegrationEvents;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync(IIntegrationEvent integrationEvent);
}