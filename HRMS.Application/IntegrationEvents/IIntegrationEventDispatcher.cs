using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Application.IntegrationEvents
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync(IIntegrationEvent integrationEvent);
    }
}
