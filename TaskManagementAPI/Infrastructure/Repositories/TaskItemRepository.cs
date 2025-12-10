using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Core.Domain.Entities;
using TaskManagementAPI.Infrastructure.Data;

namespace TaskManagementAPI.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly ApplicationDbContext _context;

    public TaskItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.TaskItems.FindAsync(id);
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem)
    {
        await _context.TaskItems.AddAsync(taskItem);
        return taskItem;
    }

    public Task UpdateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
