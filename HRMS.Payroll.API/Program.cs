using HRMS.Payroll.Application.IntegrationEvents;
using HRMS.Payroll.Application.Queries.GetEmployeePayroll;
using HRMS.Payroll.Domain.Repositories;
using HRMS.Payroll.Infrastructure.Messaging;
using HRMS.Payroll.Infrastructure.Persistence;
using HRMS.Payroll.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Payroll
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeePayrollRepository,EmployeePayrollRepository>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(GetEmployeePayrollQueryHandler).Assembly));

// RabbitMQ Consumer
builder.Services.AddScoped<PayrollEmployeeCreatedHandler>();
builder.Services.AddHostedService<PayrollRabbitMqConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();