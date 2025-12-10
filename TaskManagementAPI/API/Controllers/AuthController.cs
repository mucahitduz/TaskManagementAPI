using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.API.DTOs;
using TaskManagementAPI.Application.Services;

namespace TaskManagementAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        _logger.LogInformation("User registration attempt: {Email}", registerDto.Email);

        var result = await _authService.RegisterAsync(registerDto);

        if (result == null)
        {
            _logger.LogWarning("Registration failed: Username or email already exists");
            return BadRequest(new { message = "Username or email already exists" });
        }

        _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
        return Ok(result);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        _logger.LogInformation("User login attempt: {Email}", loginDto.Email);

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            _logger.LogWarning("Login failed: Invalid credentials for {Email}", loginDto.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
        return Ok(result);
    }
}