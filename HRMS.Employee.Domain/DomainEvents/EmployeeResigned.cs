namespace HRMS.Employee.Domain.DomainEvents
{
    public sealed record EmployeeResigned(Guid EmployeeId,DateTime ResignationDate);
}
