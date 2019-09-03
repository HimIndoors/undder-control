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
    public interface IUserApi
    {
        [Get("/user/{id}")]
        Task<HttpResponseMessage> GetUserByToken(string id);

        [Post("/user/")]
        Task<HttpResponseMessage> CreateUser([Body] UserDto user);

        [Put("/user/")]
        Task<HttpResponseMessage> UpdateUser([Body] UserDto user);
    }
}
