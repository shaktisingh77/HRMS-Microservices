using HRMS.Employee.Application.Commands.ResignEmployee;
using HRMS.Employee.Application.Outbox;
using HRMS.Employee.Domain.Repositories;
using HRMS.Employee.Infrastructure.Messaging;
using HRMS.Employee.Infrastructure.Outbox;
using HRMS.Employee.Infrastructure.Persistence;
using HRMS.Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Employee
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOutboxWriter, OutboxWriter>();
builder.Services.AddScoped<RabbitMqPublisher>();

builder.Services.AddHostedService<OutboxProcessor>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(ResignEmployeeCommandHandler).Assembly));

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