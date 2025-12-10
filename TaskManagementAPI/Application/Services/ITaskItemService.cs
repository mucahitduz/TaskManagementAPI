using TaskManagementAPI.API.DTOs;

namespace TaskManagementAPI.Application.Services;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItemDto>> GetAllTasksAsync();

    Task<TaskItemDto?> GetTaskByIdAsync(int id);

    Task<TaskItemDto> CreateTaskAsync(CreateTaskDto createTaskDto);

    Task<bool> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);

    Task<bool> DeleteTaskAsync(int id);
}
