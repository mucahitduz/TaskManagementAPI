using TaskManagementAPI.API.DTOs;

namespace TaskManagementAPI.Application.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);

    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
}