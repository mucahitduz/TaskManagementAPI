using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Core.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; } = string.Empty;

    [MaxLength(100)]
    public required string Email { get; set; } = string.Empty;

    [MaxLength(500)]
    public required string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}