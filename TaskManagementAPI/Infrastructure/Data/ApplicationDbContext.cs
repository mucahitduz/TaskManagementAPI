using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Core.Domain.Entities;

namespace TaskManagementAPI.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User> Users { get; set; }
}