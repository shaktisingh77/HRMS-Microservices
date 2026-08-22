using Microsoft.EntityFrameworkCore;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Infrastructure.Persistence;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
    }

    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EmployeeDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}