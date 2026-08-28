namespace HRMS.Leave.Domain.Entities;

public class LeaveAccount
{
    public Guid EmployeeId { get; private set; }

    public int AvailableLeave { get; private set; }

    private LeaveAccount()
    {
    }

    public LeaveAccount(Guid employeeId)
    {
        EmployeeId = employeeId;
        AvailableLeave = 0;
    }
}