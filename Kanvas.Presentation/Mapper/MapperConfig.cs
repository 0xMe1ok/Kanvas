using AutoMapper;
using Presentation.DTOs;
using Presentation.DTOs.BoardColumn;
using Presentation.DTOs.TaskBoard;
using Presentation.DTOs.Team;
using Presentation.Entities;

namespace Presentation.Mapper;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<AppTask, AppTaskDto>().ReverseMap();
        CreateMap<CreateAppTaskRequestDto, AppTask>();
        CreateMap<UpdateAppTaskRequestDto, AppTask>();
        
        CreateMap<AppTeam, AppTeamDto>().ReverseMap();
        CreateMap<CreateTeamRequestDto, AppTeam>();
        CreateMap<UpdateTeamRequestDto, AppTeam>();
        CreateMap<CreateTaskBoardInTeamRequestDto, AppTeam>();
        
        CreateMap<BoardColumn, BoardColumnDto>().ReverseMap();
        CreateMap<CreateBoardColumnRequestDto, BoardColumn>();
        CreateMap<UpdateBoardColumnRequestDto, BoardColumn>();
        
        CreateMap<TaskBoard, TaskBoardDto>().ReverseMap();
        CreateMap<CreateTaskBoardRequestDto, TaskBoard>();
        CreateMap<UpdateTaskBoardRequestDto, TaskBoard>();
    }
}