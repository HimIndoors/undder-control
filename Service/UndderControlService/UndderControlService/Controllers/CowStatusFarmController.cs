using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;

namespace UndderControlService.Controllers
{
    public class CowStatusFarmController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static int _calvingThresholdDays = -80;

        // GET api/<controller>/5
        /// Only records with both dry-off and calving dates should be used to avoid half filled records.
        /// There should be roughly 60 days between the 2 events, they should be included in the same year, if they do overlap a year (e.g December - Feb) use the calving year for the data.
        /// Currently allowing results with dryoff dates upto 80 days older than calving
        /// <summary>
        /// Get all cows by Farm ID, filtered by calendar year rules.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<CowStatusDto></returns>
        [SwaggerOperation("GetCowsStatusByFarmID")]
        [ResponseType(typeof(IEnumerable<CowStatusDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No cow status found for this farm.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified cow status are being returned.", typeof(IEnumerable<CowStatusDto>))]
        public IHttpActionResult Get(int id)
        {
            var thisYear = DateTime.Now.Year;
            var latestCowStatus = db.CowStatus.DefaultIfEmpty(null).Where(c => c.Farm_ID == id && c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == thisYear).OrderByDescending(c => c.DateAddedCalving.Value).FirstOrDefault();

            if (latestCowStatus != null)
            {
                var thresholdDate = latestCowStatus.DateAddedCalving.Value.AddDays(_calvingThresholdDays);
                var value = db.CowStatus.DefaultIfEmpty(null).Where(
                    c => c.Farm_ID == id
                    && c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == thisYear
                    && c.DateAddedDryOff.HasValue && (c.DateAddedDryOff.Value >= thresholdDate)
                ).ToList();

                if (value!=null)
                {
                    var result = AutoMapper.Mapper.Map<List<CowStatusDto>>(value);
                    Logger.Info("Returning {number} of CowStatus", value.Count);
                    return Ok(result);
                }

                Logger.Info("CowStatus data for {id} not found", id);
                return NotFound();
                
            }

            Logger.Info("Latest CowStatus data not found");
            return NotFound();            
        }

        // GET api/<controller>/5/2019
        /// Only records with both dry-off and calving dates should be used to avoid half filled records.
        /// There should be roughly 60 days between the 2 events, they should be included in the same year, if they do overlap a year (e.g December - Feb) use the calving year for the data.
        /// Currently allowing results with dryoff dates upto 80 days older than calving
        /// <summary>
        /// Get all cows by Farm ID and year, filtered by calendar year rules.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<CowStatusDto></returns>
        [SwaggerOperation("GetCowsStatusByFarmIDandYear")]
        [ResponseType(typeof(IEnumerable<CowStatusDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No cow status found for this farm and year.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified cow status are being returned.", typeof(IEnumerable<CowStatusDto>))]
        [Route("api/cowstatusfarm/{id}/{year}")]
        public IHttpActionResult Get(int id, int year)
        {
            var latestCowStatus = db.CowStatus.DefaultIfEmpty(null).Where(c => c.Farm_ID == id && c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == year).OrderByDescending(c => c.DateAddedCalving.Value).First();

            if (latestCowStatus != null)
            {
                var thresholdDate = latestCowStatus.DateAddedCalving.Value.AddDays(_calvingThresholdDays);
                var value = db.CowStatus.DefaultIfEmpty(null).Where(
                    c => c.Farm_ID == id
                    && c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == year
                    && c.DateAddedDryOff.HasValue && (c.DateAddedDryOff.Value > thresholdDate)
                ).ToList();

                if (value != null)
                {
                    var result = AutoMapper.Mapper.Map<List<CowStatusDto>>(value);
                    Logger.Info("Returning {number} of CowStatus", value.Count);
                    return Ok(result);
                }

                Logger.Info("CowStatus data for {id} not found", id);
                return NotFound();

            }

            Logger.Info("Latest CowStatus data not found");
            return NotFound();
        }
    }
}
