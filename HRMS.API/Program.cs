using HRMS.Contracts.Employee;
using HRMS.Contracts.IntegrationEvents;
using HRMS.Employee.Application.Commands.ResignEmployee;
using HRMS.Employee.Domain.Repositories;
using HRMS.Employee.Infrastructure.IntegrationEvents;
using HRMS.Employee.Infrastructure.Persistence;
using HRMS.Employee.Infrastructure.Repositories;
using HRMS.Payroll.Application.IntegrationEvents;
using HRMS.Payroll.Application.Queries.GetEmployeePayroll;
using HRMS.Payroll.Domain.Repositories;
using HRMS.Payroll.Infrastructure.Persistence;
using HRMS.Payroll.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Employee registrations
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//Scan this assembly and find my handlers
builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(ResignEmployeeCommandHandler).Assembly));
//Integration Events regsitration
builder.Services.AddScoped<IIntegrationEventDispatcher,IntegrationEventDispatcher>();

builder.Services.AddScoped<IIntegrationEventHandler<EmployeeResignedIntegrationEvent>,
                                                    PayrollEmployeeResignedHandler>();

builder.Services.AddScoped<IIntegrationEventHandler<EmployeeCreatedIntegrationEvent>, 
                                                    PayrollEmployeeCreatedHandler>();
//Payroll realated registrations 
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeePayrollRepository,EmployeePayrollRepository>();

builder.Services.AddScoped<GetEmployeePayrollQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
