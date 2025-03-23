using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using Presentation.DTOs.BoardColumn;
using Presentation.DTOs.TaskBoard;
using Presentation.Entities;
using Presentation.Enums;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId}/boards")]
[ApiVersion("1.0")]
public class TaskBoardController : ControllerBase
{
    private readonly ITaskBoardService _taskBoardService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskBoardController(
        ITaskBoardService taskBoardService,
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _taskBoardService = taskBoardService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid teamId, [FromRoute] Guid id)
    {
        var board = await _taskBoardService.GetBoardAsync(teamId, id);
        return Ok(_mapper.Map<TaskBoardDto>(board));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromRoute] Guid teamId)
    {
        var boards = await _taskBoardService.GetBoardsAsync(teamId);
        return Ok(_mapper.Map<List<TaskBoardDto>>(boards));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid teamId, [FromBody] CreateTaskBoardDto taskBoardDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var board = await _taskBoardService.CreateNewBoard(teamId, taskBoardDto);
        return CreatedAtAction(nameof(GetById), new {id = board.Id}, _mapper.Map<TaskBoardDto>(board));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid teamId,
        [FromRoute] Guid id, 
        [FromBody] UpdateTaskBoardDto boardDto)
    {
        // TODO: only from selected and accessible team, for admins/redactors
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _taskBoardService.UpdateBoardAsync(teamId, id, boardDto);
        return Ok(boardDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid teamId, [FromRoute] Guid id)
    {
        // TODO: only from selected and accessible team, for admins/redactors
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _taskBoardService.DeleteBoardAsync(teamId, id);
        return NoContent();
    }
}