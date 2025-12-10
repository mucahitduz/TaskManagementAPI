using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.API.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;
}
