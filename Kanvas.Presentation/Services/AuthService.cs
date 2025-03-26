using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.Account;
using Presentation.Exceptions;
using Presentation.Identity;
using Presentation.Identity.Tokens;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;
using Presentation.Interfaces.Service;

namespace Presentation.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserContext _userContext;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    // TODO: Change to UoW
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        UserManager<AppUser> userManager, 
        ITokenService tokenService, 
        SignInManager<AppUser> signInManager,
        IRefreshTokenRepository refreshTokenRepository,
        IUserContext userContext)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
        _userContext = userContext;
    }
    
    public async Task<LoginUserDto> Register(RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };
                
        var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
        if (!createUser.Succeeded)
            throw new ApplicationException($"Failed to create user: {createUser.Errors.FirstOrDefault()?.Description}");
                
        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded)
            throw new ApplicationException($"Failed to create user: {createUser.Errors.FirstOrDefault()?.Description}");

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = appUser.Id,
            Token = _tokenService.CreateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(7),
        };
            
        await _refreshTokenRepository.AddAsync(refreshToken);

        return new LoginUserDto
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Token = _tokenService.CreateToken(appUser),
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<LoginUserDto> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        
        if (user == null) throw new UnauthorizedException("User not found");
        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) 
            throw new UnauthorizedException("User not found");
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) throw new UnauthorizedException("User or password is incorrect");
        
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = _tokenService.CreateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(7),
        };
            
        await _refreshTokenRepository.AddAsync(refreshToken);
        return new LoginUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                RefreshToken = refreshToken.Token
            };
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<TokensDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
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
        
        return tokens;
    }

    public async Task RevokeRefreshToken()
    {
        var userId = _userContext.UserId;
        await _refreshTokenRepository.RemoveAsync(userId);
    }
}