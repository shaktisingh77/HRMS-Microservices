using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeEntity = HRMS.Employee.Domain.Entities.Employee;

namespace HRMS.Employee.Infrastructure.Persistence.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(e => e.EmployeeId);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(e => e.DepartmentId)
            .IsRequired();

        builder.Property(e => e.JoiningDate)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.OwnsOne(e => e.ResignationDate,
                        resignationDate =>
                        {
                        resignationDate.Property(r => r.Value)
                        .HasColumnName("ResignationDate");
                        });
    }
}