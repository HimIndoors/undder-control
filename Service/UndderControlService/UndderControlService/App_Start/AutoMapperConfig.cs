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
                cfg.CreateMap<CowStatus, CowStatusDto>();
                cfg.CreateMap<Farm, FarmDto>();
                cfg.CreateMap<Survey, SurveyDto>();
                cfg.CreateMap<SurveyQuestion, SurveyQuestionDto>();
                cfg.CreateMap<SurveyStage, SurveyStageDto>();
                cfg.CreateMap<SurveyResponse, SurveyResponseDto>();
                cfg.CreateMap<SurveyQuestionResponse, SurveyQuestionResponseDto>();
                cfg.CreateMap<User, UserDto>();

                cfg.CreateMap<CowStatusDto, CowStatus>();
                cfg.CreateMap<FarmDto, Farm>();
                cfg.CreateMap<SurveyDto, Survey>();
                cfg.CreateMap<SurveyQuestionDto, SurveyQuestion>();
                cfg.CreateMap<SurveyStageDto, SurveyStage>();
                cfg.CreateMap<SurveyResponseDto, SurveyResponse>();
                cfg.CreateMap<SurveyQuestionResponseDto, SurveyQuestionResponse>();
                cfg.CreateMap<UserDto, User>();
            });
        }
    }
}