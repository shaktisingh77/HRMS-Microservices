using HRMS.Leave.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Leave.Infrastructure.Persistence;

public class LeaveDbContext : DbContext
{
    public LeaveDbContext(DbContextOptions<LeaveDbContext> options)  : base(options)
    {
    }

    public DbSet<LeaveAccount> LeaveAccounts => Set<LeaveAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaveAccount>().HasKey(x => x.EmployeeId);

        base.OnModelCreating(modelBuilder);
    }
}