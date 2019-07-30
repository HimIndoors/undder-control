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
    public interface ISurveyApi
    {
        [Post("/survey/")]
        Task<HttpResponseMessage> UploadSurvey([Body] SurveyResponseDto survey);
    }
}
