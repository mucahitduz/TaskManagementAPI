using BCrypt.Net;
using TaskManagementAPI.API.DTOs;
using TaskManagementAPI.Core.Domain.Entities;
using TaskManagementAPI.Infrastructure.Repositories;

namespace TaskManagementAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var existingUserByEmail = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUserByEmail != null)
            return null;

        var existingUserByUsername = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUserByUsername != null)
            return null; 

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);
        var expirationInMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"]!);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationInMinutes)
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
            return null;

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
            return null;

        var token = _jwtService.GenerateToken(user);
        var expirationInMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"]!);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationInMinutes)
        };
    }
}