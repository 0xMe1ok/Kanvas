using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Interfaces;
using Presentation.Interfaces.Service;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/teams/{teamId:guid}/boards/{boardId:guid}/columns")]
[ApiVersion("1.0")]
[Authorize]
public class BoardColumnController : Controller
{
    private readonly IBoardColumnService _columnService;
    private readonly IMapper _mapper;

    public BoardColumnController(
        IMapper mapper, 
        IBoardColumnService columnService)
    {
        _columnService = columnService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromRoute] Guid teamId, [FromRoute] Guid boardId)
    {
        var columns = await _columnService.GetColumnsAsync(teamId, boardId);
        return Ok(_mapper.Map<List<BoardColumnDto>>(columns));
    }

    [HttpPost]
    public async Task<ActionResult> Create(
        [FromRoute] Guid teamId,
        [FromRoute] Guid boardId, 
        [FromBody] CreateBoardColumnDto columnDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var column = await _columnService.CreateNewColumn(teamId, boardId, columnDto);
        return CreatedAtAction(nameof(GetAll), new { teamId, boardId }, column);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult> Update(
        [FromRoute] Guid teamId, 
        [FromRoute] Guid boardId, 
        [FromRoute] Guid id,
        [FromBody] UpdateBoardColumnDto boardColumnDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _columnService.UpdateColumnAsync(teamId, boardId, id, boardColumnDto);
        return Ok(boardColumnDto);
    }
    
    [HttpPatch]
    [Route("{id:guid}/order")]
    public async Task<IActionResult> Move(
        [FromRoute] Guid teamId,
        [FromRoute] Guid boardId, 
        [FromRoute] Guid id, 
        [FromBody] MoveBoardColumnDto columnDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _columnService.MoveColumnAsync(teamId, boardId, id, columnDto.NewOrder);
        return Ok(columnDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid teamId, 
        [FromRoute] Guid boardId, 
        [FromRoute] Guid id)
    {
        await _columnService.DeleteColumnAsync(teamId, boardId, id);
        return NoContent();
    }
}