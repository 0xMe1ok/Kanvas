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
        CreateMap<CreateAppTaskDto, AppTask>();
        CreateMap<UpdateAppTaskDto, AppTask>();
        
        CreateMap<AppTeam, AppTeamDto>().ReverseMap();
        CreateMap<CreateAppTeamDto, AppTeam>();
        CreateMap<UpdateAppTeamDto, AppTeam>();
        CreateMap<CreateTaskBoardInTeamDto, AppTeam>();
        
        CreateMap<BoardColumn, BoardColumnDto>().ReverseMap();
        CreateMap<CreateBoardColumnDto, BoardColumn>();
        CreateMap<UpdateBoardColumnDto, BoardColumn>();
        
        CreateMap<TaskBoard, TaskBoardDto>().ReverseMap();
        CreateMap<CreateTaskBoardDto, TaskBoard>();
        CreateMap<UpdateTaskBoardDto, TaskBoard>();
    }
}