using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Application.Commands.ResignEmployee;
using HRMS.Employee.Application.Outbox;
using HRMS.Employee.Domain.Repositories;
using HRMS.Employee.Infrastructure.IntegrationEvents;
using HRMS.Employee.Infrastructure.Messaging;
using HRMS.Employee.Infrastructure.Outbox;
using HRMS.Employee.Infrastructure.Persistence;
using HRMS.Employee.Infrastructure.Repositories;
using HRMS.Leave.Application.IntegrationEvents;
using HRMS.Leave.Domain.Repositories;
using HRMS.Leave.Infrastructure.Messaging;
using HRMS.Leave.Infrastructure.Persistence;
using HRMS.Leave.Infrastructure.Repositories;
using HRMS.Payroll.Application.IntegrationEvents;
using HRMS.Payroll.Application.Queries.GetEmployeePayroll;
using HRMS.Payroll.Domain.Repositories;
using HRMS.Payroll.Infrastructure.Messaging;
using HRMS.Payroll.Infrastructure.Persistence;
using HRMS.Payroll.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ============================================================
// Employee registrations
// ============================================================

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IOutboxWriter, OutboxWriter>();

builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddScoped<RabbitMqPublisher>();


// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(ResignEmployeeCommandHandler).Assembly));


// Integration Event Dispatcher
builder.Services.AddScoped<IIntegrationEventDispatcher,IntegrationEventDispatcher>();


// Employee Resigned event handler
builder.Services.AddScoped<IIntegrationEventHandler<EmployeeResignedIntegrationEvent>, PayrollEmployeeResignedHandler>();


// ============================================================
// Payroll registrations
// ============================================================

builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeePayrollRepository,EmployeePayrollRepository>();

builder.Services.AddScoped<GetEmployeePayrollQueryHandler>();

// Payroll RabbitMQ handler
builder.Services.AddScoped<PayrollEmployeeCreatedHandler>();

// Payroll RabbitMQ consumer
builder.Services.AddHostedService<PayrollRabbitMqConsumer>();

// ============================================================
// Leave registrations
// ============================================================

builder.Services.AddDbContext<LeaveDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<ILeaveAccountRepository,LeaveAccountRepository>();


// Leave RabbitMQ handler
builder.Services.AddScoped<LeaveEmployeeCreatedHandler>();

// Leave RabbitMQ consumer
builder.Services.AddHostedService<LeaveRabbitMqConsumer>();

var app = builder.Build();

// ============================================================
// HTTP request pipeline
// ============================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();