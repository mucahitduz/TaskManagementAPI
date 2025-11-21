using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Core.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    [MaxLength(200)]
    public required string Title { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
