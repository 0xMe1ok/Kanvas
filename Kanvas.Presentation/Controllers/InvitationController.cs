using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Invitation;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Exceptions;
using Presentation.Extensions;
using Presentation.Identity;
using Presentation.Interfaces;
using Presentation.Interfaces.Repository;
using Presentation.Repositories;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/invitation")]
[ApiVersion("1.0")]
[Authorize]
public class InvitationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IInviteTokenRepository _inviteTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InvitationController(
        IMapper mapper, 
        UserManager<AppUser> userManager, 
        ITokenService tokenService, 
        SignInManager<AppUser> signInManager,
        IInviteTokenRepository inviteTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _inviteTokenRepository = inviteTokenRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateToken(GenerateInvitationDto invitationDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var inviteToken = new InviteToken()
        {
            Id = Guid.NewGuid(),
            TeamId = invitationDto.TeamId,
            Expires = DateTime.UtcNow.AddHours(1),
            Token = _tokenService.CreateInvitationToken()
        };
        
        await _inviteTokenRepository.AddAsync(inviteToken);
        
        return Ok(inviteToken.Token);
    }
    
    [HttpPost]
    [Route("activate")]
    [Authorize]
    public async Task<IActionResult> ActivateToken([FromBody] ActivateInvitationDto invitationDto)
    {
        // TODO: Move logic in service
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null) return Unauthorized();
        
        var inviteToken = await _inviteTokenRepository.GetAsync(invitationDto.InviteToken);
        if (inviteToken == null) throw new NotFoundException("Invite token is invalid");

        var teamMember = new TeamMember()
        {
            MemberId = appUser.Id,
            TeamId = inviteToken.TeamId,
            Role = TeamRole.Viewer
        };

        await _unitOfWork.TeamMembers.AddAsync(teamMember);
        
        return Ok(teamMember);
    }
}