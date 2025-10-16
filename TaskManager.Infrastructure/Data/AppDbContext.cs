using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<TaskItemHistory> TaskHistories => Set<TaskItemHistory>();
    public DbSet<TaskItemComment> TaskComments => Set<TaskItemComment>();
}