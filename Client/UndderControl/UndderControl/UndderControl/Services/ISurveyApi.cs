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
        [Get("/survey?activeOnly=true")]
        Task<HttpResponseMessage> GetLatestSurvey();
    }
}
