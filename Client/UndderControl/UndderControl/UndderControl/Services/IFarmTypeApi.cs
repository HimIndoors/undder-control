using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UndderControl.Services
{
    [Headers("Content-Type: application/json")]
    public interface IFarmTypeApi
    {
        [Get("/farmtype")]
        Task<HttpResponseMessage> GetFarmTypes();
    }
}
