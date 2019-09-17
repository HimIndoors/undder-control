using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UndderControl.Services
{
    public interface IFarmUserApi
    {
        [Get("/farmuser/{id}")]
        Task<HttpResponseMessage> GetFarmsByUserId(int id);
    }
}
