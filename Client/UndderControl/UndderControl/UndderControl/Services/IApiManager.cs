﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UndderControlLib.Dtos;

namespace UndderControl.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> FarmList();
        Task<HttpResponseMessage> GetFarmsByUserId(int id);
        Task<HttpResponseMessage> GetLatestSurvey();
        Task<HttpResponseMessage> UploadSurvey(SurveyResponseDto survey);
        Task<HttpResponseMessage> UploadFarm(FarmDto farm, bool isNew);
        Task<HttpResponseMessage> UploadCowStatus(CowStatusDto status);

    }
}
