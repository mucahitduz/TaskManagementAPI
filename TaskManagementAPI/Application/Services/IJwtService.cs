using TaskManagementAPI.Core.Domain.Entities;

namespace TaskManagementAPI.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
