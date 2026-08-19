namespace HRMS.Employee.Domain.ValueObjects
{
    public sealed class ResignationDate
    {
        public DateTime Value { get; }
        public ResignationDate(DateTime value)
        {
            if (value.Date < DateTime.Today)
            {
                throw new ArgumentException("Resignation date cannot be in the past.");
            }
            Value = value.Date;
        }
    }
}
