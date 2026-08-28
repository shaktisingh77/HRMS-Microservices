using HRMS.Payroll.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Payroll.Infrastructure.Persistence;

public class PayrollDbContext : DbContext
{
    public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
    {
    }

    public DbSet<EmployeePayroll> EmployeePayrolls => Set<EmployeePayroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeePayroll>().HasKey(x => x.EmployeeId);

        base.OnModelCreating(modelBuilder);
    }
}