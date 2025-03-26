using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Account;
using Presentation.Extensions;
using Presentation.Identity;
using Presentation.Identity.Tokens;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;
using Presentation.Interfaces.Service;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var loginResult = await _authService.Register(registerDto);
        return Ok(loginResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var loginResult = await _authService.Login(loginDto);
        return Ok(loginResult);
    }
    
    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var tokens = await _authService.RefreshToken(refreshTokenDto);
        return Ok(tokens);
    }
    
    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken()
    {
        await _authService.RevokeRefreshToken();
        return NoContent();
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok();
    }
        
}