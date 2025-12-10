using TaskManagementAPI.Core.Domain.Entities;

namespace TaskManagementAPI.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByUsernameAsync(string username);

    Task<User> AddAsync(User user);

    Task<bool> SaveChangesAsync();
}