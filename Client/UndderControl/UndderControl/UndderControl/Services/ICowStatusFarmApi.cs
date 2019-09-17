using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UndderControl.Services
{
    public interface ICowStatusFarmApi
    {
        [Get("/cowstatusfarm/{id}")]
        Task<HttpResponseMessage> GetCowsStatusByFarmID(int id);

        [Get("/cowstatusfarm/{id}/{year}")]
        Task<HttpResponseMessage> GetCowsStatusByFarmIDandYear(int id, int year);
    }
}
