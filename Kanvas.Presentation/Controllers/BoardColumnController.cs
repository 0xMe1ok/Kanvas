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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BoardColumnController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromRoute] Guid boardId)
    {
        // TODO: only for boards in accessible teams
        var columns = await _unitOfWork.Columns
            .FindAllAsync(column => column.BoardId == boardId);
        return Ok(_mapper.Map<List<BoardColumnDto>>(columns));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromRoute] Guid boardId, [FromBody] CreateBoardColumnDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            return NotFound("Board not found");
        }

        if (await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == boardColumnDto.Status))
        {
            return Conflict("Column with that status already exists");
        }
        
        var column = _mapper.Map<BoardColumn>(boardColumnDto);
        column.BoardId = boardId;
        await _unitOfWork.Columns.AddAsync(column);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid boardId, 
        [FromRoute] Guid id, [FromBody] UpdateBoardColumnDto boardColumnDto)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        // TODO: check for valid status, there can't be two columns with the same status
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            return NotFound("Board not found");
        }
        
        if (await _unitOfWork.Columns.ExistsAsync
            (c => c.BoardId == boardId &&
                  c.Status == boardColumnDto.Status))
        {
            return Conflict("Column with that status already exists");
        }
        
        var column = await _unitOfWork.Columns.GetByIdAsync(id);
        if (column == null) return NotFound();
        _mapper.Map(boardColumnDto, column);
        await _unitOfWork.CommitAsync();
        
        return Ok(_mapper.Map<BoardColumnDto>(column));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid boardId, [FromRoute] Guid id)
    {
        // TODO: only for boards in accessible teams, for admins/redactors
        
        if (!await _unitOfWork.Boards.ExistsAsync(boardId))
        {
            return NotFound("Board by id does not exist");
        }
        
        var column = await _unitOfWork.Columns.GetByIdAsync(id);
        if (column == null) return NotFound("Column by id does not exist");
        _unitOfWork.Columns.Remove(column);

        await _unitOfWork.Tasks.ClearColumnIdInColumnAsync(id);
        await _unitOfWork.CommitAsync();
        
        return NoContent();
    }
}