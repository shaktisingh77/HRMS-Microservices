using HRMS.Employee.Domain.Enums;
using HRMS.Employee.Domain.ValueObjects;

namespace HRMS.Employee.Domain.Entities;
public class Employee 
{
    public Guid EmployeeId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Guid DepartmentId { get; private set; }
    public DateTime JoiningDate { get; private set; }
    public EmploymentStatus Status { get; private set; }
    public ResignationDate? ResignationDate { get; private set; }

    public Employee(Guid employeeId,string name,string email, Guid departmentId, DateTime joiningDate)
    {
        EmployeeId = employeeId;
        Name = name;
        Email = email;
        DepartmentId = departmentId;
        JoiningDate = joiningDate;
        Status = EmploymentStatus.Active;
    }

    public void Resign(DateTime resignationDate)
    {
        if (Status == EmploymentStatus.Resigned)
        {
            throw new InvalidOperationException("Employee has already resigned.");
        }
        if (Status == EmploymentStatus.Terminated)
        {
            throw new InvalidOperationException("Terminated employee cannot resign.");
        }

        if (resignationDate.Date < DateTime.Today)
        {
            throw new ArgumentException(
                "Resignation date cannot be in the past.");
        }

        ResignationDate = new ResignationDate(resignationDate);
        Status = EmploymentStatus.Resigned;        
    }
}