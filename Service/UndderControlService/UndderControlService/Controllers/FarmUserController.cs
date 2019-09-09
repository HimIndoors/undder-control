using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;

namespace UndderControlService.Controllers
{
    public class FarmUserController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>/5
        /// <summary>
        /// Get farms by User ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<FarmDto></returns>
        [SwaggerOperation("GetFarmsByUserID")]
        [ResponseType(typeof(IEnumerable<FarmDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No farms with the specified User ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farms are being returned.", typeof(IEnumerable<FarmDto>))]
        public IHttpActionResult Get(int id)
        {
            var value = db.Farms.DefaultIfEmpty(null).Where(f => f.User_ID == id).ToList();
            if(value != null)
            {
                var farms = AutoMapper.Mapper.Map<List<FarmDto>>(value);
                Logger.Info("Returning {count} farms for user {id}", value.Count, id);
                return Ok(farms);
            }

            Logger.Info("No farms found for this user {id}", id);
            return NotFound();
        }
    }
}
