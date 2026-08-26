using HRMS.Contracts.IntegrationEvents;

namespace HRMS.Contracts.Employee;

public sealed record EmployeeResignedIntegrationEvent(Guid EmployeeId,
                                                      DateTime ResignationDate) : IIntegrationEvent;