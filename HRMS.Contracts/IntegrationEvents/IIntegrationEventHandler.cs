namespace HRMS.Contracts.IntegrationEvents;

public interface IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent integrationEvent);
}