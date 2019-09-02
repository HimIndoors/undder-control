using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UndderControlLib.Dtos;

namespace UndderControl.Services
{
    [Headers("Content-Type: application/json")]
    public interface ISurveyResponseApi
    {
        [Get("/farm/{id}")]
        Task<HttpResponseMessage> GetResponseByFarmId(int id);

        [Post("/survey/")]
        Task<HttpResponseMessage> UploadSurvey([Body] SurveyResponseDto survey);
    }
}
