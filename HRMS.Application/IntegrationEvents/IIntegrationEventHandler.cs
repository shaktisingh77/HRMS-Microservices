using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Application.IntegrationEvents
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
    {
        Task HandleAsync(TEvent integrationEvent);
    }
}
