using HRMS.Employee.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Infrastructure.Persistence;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
    }

    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}