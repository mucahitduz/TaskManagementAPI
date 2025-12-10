using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.API.DTOs;
using TaskManagementAPI.Application.Services;

namespace TaskManagementAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _service;
    private readonly ILogger<TaskItemsController> _logger;

    public TaskItemsController(
        ITaskItemService service,
        ILogger<TaskItemsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: api/taskitems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAllTasks()
    {
        _logger.LogInformation("Getting all tasks");

        var taskDtos = await _service.GetAllTasksAsync();

        return Ok(taskDtos);
    }

    // GET: api/taskitems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetTaskById(int id)
    {
        _logger.LogInformation("Getting task with ID: {TaskId}", id);

        var taskDto = await _service.GetTaskByIdAsync(id);

        if (taskDto == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", id);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        return Ok(taskDto);
    }

    // POST: api/taskitems
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTask(CreateTaskDto createTaskDto)
    {
        _logger.LogInformation("Creating new task: {TaskTitle}", createTaskDto.Title);

        var taskDto = await _service.CreateTaskAsync(createTaskDto);

        _logger.LogInformation("Task created with ID: {TaskId}", taskDto.Id);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = taskDto.Id },
            taskDto);
    }

    // PUT: api/taskitems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateTaskDto)
    {
        _logger.LogInformation("Updating task with ID: {TaskId}", id);

        var success = await _service.UpdateTaskAsync(id, updateTaskDto);

        if (!success)
        {
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        _logger.LogInformation("Task with ID {TaskId} updated successfully", id);

        return NoContent();
    }

    // DELETE: api/taskitems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", id);

        var success = await _service.DeleteTaskAsync(id);

        if (!success)
        {
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        _logger.LogInformation("Task with ID {TaskId} deleted successfully", id);

        return NoContent();
    }
}
