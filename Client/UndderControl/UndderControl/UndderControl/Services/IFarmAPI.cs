﻿using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UndderControlLib.Dtos;

namespace UndderControl.Services
{
    public interface IFarmApi
    {
        [Get("/farm/")]
        Task<HttpResponseMessage> FarmList();

        [Get("/farm/{id}")]
        Task<FarmDto> GetFarmById(Guid id);

        [Post("/farm/")]
        Task CreateFarm([Body] FarmDto farm);

        [Put("/farm/{id}")]
        Task UpdateFarm(Guid id, [Body] FarmDto farm);
    }
}