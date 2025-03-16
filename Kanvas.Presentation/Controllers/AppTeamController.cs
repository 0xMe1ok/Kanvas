using Asp.Versioning;
using AutoMapper;
using Presentation;
using Presentation.DTOs;
using Presentation.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs.TaskBoard;
using Presentation.DTOs.Team;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams")]
[ApiVersion("1.0")]
public class AppTeamController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppTeamController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // TODO: get all from accessible teams
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) return NotFound();
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: only for accessible teams
        var teams = await _unitOfWork.Teams.GetAllAsync();
        return Ok(teams.Select(team => _mapper.Map<AppTeamDto>(team)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequestDto appTeamDto)
    {
        // TODO: use userId to ownerId
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = _mapper.Map<AppTeam>(appTeamDto);
        await _unitOfWork.Teams.AddAsync(team);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTeamRequestDto appTeamDto)
    {
        // TODO: only for owner
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) return NotFound();
        _mapper.Map(appTeamDto, team);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<AppTeamDto>(team));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // TODO: only for owner
        var team = await _unitOfWork.Teams.GetByIdAsync(id);
        if (team == null) return NotFound();
        _unitOfWork.Teams.Remove(team);
        await _unitOfWork.CommitAsync();
        
        return NoContent();
    }

    [HttpGet]
    [Route("{id:guid}/boards")]
    public async Task<IActionResult> GetBoards([FromRoute] Guid id)
    {
        var boards = await _unitOfWork.Boards.FindAllAsync(board => board.TeamId == id);
        return Ok(_mapper.Map<IEnumerable<TaskBoardDto>>(boards));
    }
    
    [HttpPost]
    [Route("{id:guid}/boards")]
    public async Task<IActionResult> CreateBoard([FromRoute] Guid id, [FromBody] CreateTaskBoardInTeamRequestDto boardDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (await _unitOfWork.Teams.ExistsAsync(id))
        {
            return NotFound("Team doesn't exist");
        }
        
        var board = _mapper.Map<TaskBoard>(boardDto);
        board.TeamId = id;
        await _unitOfWork.Boards.AddAsync(board);
        
        // TODO: move to service
        var columnsStarterPack = new List<BoardColumn>
        {
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "ToDo",
                Order = 0,
                Status = Status.ToDo,
                TaskLimit = null
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "InProgress",
                Order = 1,
                Status = Status.InProgress,
                TaskLimit = null
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                Name = "Done",
                Order = 2,
                Status = Status.Done,
                TaskLimit = null
            },
        };

        await _unitOfWork.Columns.AddRangeAsync(columnsStarterPack);
        await _unitOfWork.CommitAsync();
        
        return CreatedAtAction(nameof(GetById), new {id = board.Id}, _mapper.Map<TaskBoardDto>(board));
    }
    
    // TODO: do when user was added
    [HttpPost]
    [Route("{teamId}/users")] // [Route("{teamId}/users")]
    public async Task<IActionResult> AddUserToTeam([FromRoute] Guid teamId, [FromRoute] Guid userId) // FromBody] user UsedId
    {
        // TODO: only for owner
        return Ok();
    }
    
    // TODO: do when user was added
    [HttpDelete]
    [Route("{teamId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromTeam([FromRoute] Guid teamId, [FromRoute] Guid userId)
    {
        // TODO: only for owner
        return Ok();
    }
    
}