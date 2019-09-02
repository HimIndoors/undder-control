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
    public class UserFarmsController : BaseController
    {
        // GET api/<controller>/5
        /// <summary>
        /// Get all farms by User ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<FarmDto></returns>
        [SwaggerOperation("GetFarmsByUserID")]
        [ResponseType(typeof(IEnumerable<FarmDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "A User with the specified ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farms are being returned.", typeof(IEnumerable<FarmDto>))]
        public IHttpActionResult GetFarmsByUserId(int id)
        {
            var farms = AutoMapper.Mapper.Map<List<FarmDto>>(db.Farms.Where(f => f.User_ID == id).ToList());
            return Ok(farms);
        }
    }
}
