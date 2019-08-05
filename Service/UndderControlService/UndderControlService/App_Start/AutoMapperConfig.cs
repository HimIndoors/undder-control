using AutoMapper;
using UndderControlLib.Dtos;
using UndderControlService.Data.Entities;

namespace UndderControlService.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Farm, FarmDto>();
                cfg.CreateMap<Survey, SurveyDto>();
                cfg.CreateMap<SurveyResponse, SurveyResponseDto>();

                cfg.CreateMap<FarmDto, Farm>();
                cfg.CreateMap<SurveyDto, Survey>();
                cfg.CreateMap<SurveyResponseDto, SurveyResponse>();
            });
        }
    }
}