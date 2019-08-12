using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using UndderControlLib.Dtos;

namespace UndderControl.Services
{
    public interface ICowStatusApi
    {
        [Get("/cowstatus/{id}")]
        Task<HttpResponseMessage> GetStatusByFarmId(int id);

        [Post("/cowstatus/")]
        Task<HttpResponseMessage> CreateCowStatus([Body] CowStatusDto farm);
    }
}
