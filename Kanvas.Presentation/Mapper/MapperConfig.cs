using AutoMapper;
using Presentation.DTOs;
using Presentation.Entities;

namespace Infrastructure.Mapper;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<AppTask, AppTaskDto>().ReverseMap();
        CreateMap<CreateAppTaskRequestDto, AppTask>();
        CreateMap<UpdateAppTaskRequestDto, AppTask>();
        
    }
}