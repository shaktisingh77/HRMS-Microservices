using HRMS.Leave.Application.Queries.GetLeaveAccount;
using HRMS.Leave.Application.IntegrationEvents;
using HRMS.Leave.Domain.Repositories;
using HRMS.Leave.Infrastructure.Messaging;
using HRMS.Leave.Infrastructure.Persistence;
using HRMS.Leave.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Leave
builder.Services.AddDbContext<LeaveDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<ILeaveAccountRepository, LeaveAccountRepository>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(GetLeaveAccountQueryHandler).Assembly));

// RabbitMQ Consumer
builder.Services.AddScoped<LeaveEmployeeCreatedHandler>();
builder.Services.AddHostedService<LeaveRabbitMqConsumer>();

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