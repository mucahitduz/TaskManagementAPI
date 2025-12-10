using TaskManagementAPI.Core.Domain.Entities;

namespace TaskManagementAPI.Infrastructure.Repositories;

public interface ITaskItemRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();

    Task<TaskItem?> GetByIdAsync(int id);

    Task<TaskItem> AddAsync(TaskItem taskItem);

    Task UpdateAsync(TaskItem taskItem);

    Task DeleteAsync(TaskItem taskItem);

    Task<bool> SaveChangesAsync();
}
