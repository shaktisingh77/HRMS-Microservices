using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Contracts.Employee;

public sealed record EmployeeCreatedIntegrationEvent(Guid EmployeeId) : IIntegrationEvent;