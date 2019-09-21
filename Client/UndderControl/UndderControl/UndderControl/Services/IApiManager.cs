using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UndderControlLib.Dtos;

namespace UndderControl.Services
{
    public interface IApiManager
    {
        #region Farm methods
        Task<HttpResponseMessage> FarmList();
        Task<HttpResponseMessage> GetFarmById(int id);
        Task<HttpResponseMessage> UploadFarm(FarmDto farm, bool isNew);
        #endregion Farm methods

        #region Farm By User methods
        Task<HttpResponseMessage> GetFarmsByUserId(int id);
        #endregion Farm By User methods

        #region FarmType methods
        Task<HttpResponseMessage> GetFarmTypes();
        #endregion FarmType methods

        #region Survey methods
        Task<HttpResponseMessage> GetLatestSurvey();
        #endregion Survey methods

        #region SurveyResponse methods
        Task<HttpResponseMessage> GetResponseByFarmId(int id);
        Task<HttpResponseMessage> UploadResponse(SurveyResponseDto survey);
        #endregion SurveyResponse methods

        #region CowStatus methods
        Task<HttpResponseMessage> CreateCowStatus(CowStatusDto farm);
        Task<HttpResponseMessage> UpdateCowStatus(CowStatusDto status);
        Task<HttpResponseMessage> GetStatusByFarmId(int id);
        #endregion CowStatusResponse methods

        #region CowStatus By Farm methods
        Task<HttpResponseMessage> GetCowsStatusByFarmID(int id);
        Task<HttpResponseMessage> GetCowsStatusByFarmIDandYear(int id, int year);
        #endregion CowStatus By Farm methods

        #region User methods
        Task<HttpResponseMessage> GetUserByToken(string token);
        Task<HttpResponseMessage> CreateUser(UserDto user);
        Task<HttpResponseMessage> UpdateUser(UserDto user);
        #endregion User methods
    }
}
