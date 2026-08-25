namespace HRMS.Employee.Domain.ValueObjects
{
    public sealed class ResignationDate
    {
        public DateTime Value { get; }
        public ResignationDate(DateTime value)
        {            
            Value = value.Date;
        }
    }
}
