using AutoMapper;
using TaskManagementAPI.API.DTOs;
using TaskManagementAPI.Core.Domain.Entities;
using TaskManagementAPI.Infrastructure.Repositories;

namespace TaskManagementAPI.Application.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _repository;
    private readonly IMapper _mapper;

    public TaskItemService(
        ITaskItemRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync()
    {
        var tasks = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<TaskItemDto>>(tasks);
    }

    public async Task<TaskItemDto?> GetTaskByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            return null;

        return _mapper.Map<TaskItemDto>(task);
    }

    public async Task<TaskItemDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        var taskItem = _mapper.Map<TaskItem>(createTaskDto);

        taskItem.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(taskItem);
        await _repository.SaveChangesAsync();

        return _mapper.Map<TaskItemDto>(taskItem);
    }

    public async Task<bool> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
    {
        var existingTask = await _repository.GetByIdAsync(id);

        if (existingTask == null)
            return false;

        _mapper.Map(updateTaskDto, existingTask);
        
        existingTask.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingTask);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        
        if (task == null)
            return false;

        await _repository.DeleteAsync(task);
        await _repository.SaveChangesAsync();

        return true;
    }
}