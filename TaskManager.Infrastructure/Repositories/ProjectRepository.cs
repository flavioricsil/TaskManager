using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjectsByUserAsync(Guid userId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.OwnerId == userId)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task AddAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}