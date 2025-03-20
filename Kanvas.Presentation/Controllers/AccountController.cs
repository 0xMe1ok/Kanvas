using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Account;
using Presentation.Identity;
using Presentation.Identity.Tokens;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AccountController(
        UserManager<AppUser> userManager, 
        ITokenService tokenService, 
        SignInManager<AppUser> signInManager,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
                
            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createUser.Succeeded)
            {
                return StatusCode(500, createUser.Errors);
            }
                
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                return StatusCode(500, roleResult.Errors);
            }

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = appUser.Id,
                Token = _tokenService.CreateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
            };
            
            await _refreshTokenRepository.AddAsync(refreshToken);
            
            return Ok(
                new LoginUserDto
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    Token = _tokenService.CreateToken(appUser),
                    RefreshToken = refreshToken.Token
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null) return Unauthorized();
        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) return Unauthorized("Username or password is incorrect");
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenService.CreateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(7),
        };
            
        await _refreshTokenRepository.AddAsync(refreshToken);
        return Ok(
            new LoginUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                RefreshToken = refreshToken.Token
            });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var refreshToken = await _refreshTokenRepository.GetAsync(refreshTokenDto.Token);
        
        if (refreshToken == null) throw new FormatException("Refresh token is invalid");
        
        var accessToken = _tokenService.CreateToken(refreshToken.AppUser);

        refreshToken.Token = _tokenService.CreateRefreshToken();
        refreshToken.Expires = DateTime.UtcNow.AddDays(7);
        await _refreshTokenRepository.CommitAsync();

        var tokens = new TokensDto()
        {
            Token = accessToken,
            RefreshToken = refreshToken.Token
        };
        
        return Ok(tokens);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
        
}