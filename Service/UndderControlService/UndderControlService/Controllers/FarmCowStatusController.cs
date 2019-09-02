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
    public class FarmCowStatusController : BaseController
    {
        // GET api/<controller>/5
        /// <summary>
        /// Get all cows by Farm ID, filtered by calendar year.
        /// Only records with both dry-off and calving dates for the current year are returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<CowStatusDto></returns>
        [SwaggerOperation("GetCowsStatusByFarmID")]
        [ResponseType(typeof(IEnumerable<CowStatusDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No cow status found for this farm.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified cow status are being returned.", typeof(IEnumerable<CowStatusDto>))]
        public IHttpActionResult GetCowsStatusByFarmID(int id)
        {
            var thisYear = DateTime.Now.Year;
            var result = AutoMapper.Mapper.Map<List<CowStatusDto>>(db.CowStatus.Where(
                c => c.Farm_ID == id
                && (c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == thisYear)
                && (c.DateAddedDryOff.HasValue && c.DateAddedDryOff.Value.Year == thisYear)
                ).ToList());
            return Ok(result);
        }
    }
}
