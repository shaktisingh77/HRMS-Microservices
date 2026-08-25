using HRMS.Employee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HRMS.Employee.Domain.Repositories;
using HRMS.Employee.Infrastructure.Repositories;
using HRMS.Employee.Application.Commands.ResignEmployee;
using HRMS.Employee.Infrastructure.DomainEvents;
using HRMS.Employee.Application.DomainEvents;
using HRMS.Employee.Domain.DomainEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HRMS")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(ResignEmployeeCommandHandler).Assembly));

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddScoped<IDomainEventHandler<EmployeeResigned>,EmployeeResignedHandler>();

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
