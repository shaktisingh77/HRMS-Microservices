namespace HRMS.Payroll.Domain.Entities;

public class EmployeePayroll
{
    public Guid EmployeeId { get; private set; }

    public bool IsActive { get; private set; }

    private EmployeePayroll()
    {
    }

    public EmployeePayroll(Guid employeeId)
    {
        EmployeeId = employeeId;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}