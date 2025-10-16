using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Application.Validators;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();

// Serviços
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API V1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();