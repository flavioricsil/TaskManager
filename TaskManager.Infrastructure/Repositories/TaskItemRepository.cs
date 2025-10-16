using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;

    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId)
    {
        return await _context.TaskItems
            .Include(t => t.History)
            .Include(t => t.Comments)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid taskId)
    {
        return await _context.TaskItems
            .Include(t => t.History)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task AddAsync(TaskItem task)
    {
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.TaskItems.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
    }
}