using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.BoardColumn;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/boards/{boardId:guid}/columns")]
[ApiVersion("1.0")]
public class BoardColumnController : Controller
{
    private readonly IBoardColumnService _columnService;
    private readonly IMapper _mapper;

    public BoardColumnController(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IBoardColumnService columnService)
    {
        _columnService = columnService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromRoute] Guid boardId)
    {
        var columns = await _columnService.GetColumnsAsync(boardId);
        return Ok(_mapper.Map<List<BoardColumnDto>>(columns));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromRoute] Guid boardId, [FromBody] CreateBoardColumnDto columnDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var column = await _columnService.CreateNewColumn(boardId, columnDto);
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid boardId, 
        [FromRoute] Guid id, [FromBody] UpdateBoardColumnDto boardColumnDto)
    {
        
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _columnService.UpdateColumnAsync(boardId, id, boardColumnDto);
        return Ok(boardColumnDto);
    }
    
    [HttpPatch]
    [Route("{id:guid}/order")]
    public async Task<IActionResult> Move(
        [FromRoute] Guid boardId, 
        [FromRoute] Guid id, 
        [FromBody] MoveBoardColumnDto columnDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _columnService.MoveColumnAsync(boardId, id, columnDto.NewOrder);
        return Ok(columnDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid boardId, [FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        await _columnService.DeleteColumnAsync(boardId, id);
        return NoContent();
    }
}