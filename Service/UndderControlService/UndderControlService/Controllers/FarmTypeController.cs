using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;

namespace UndderControlService.Controllers
{
    public class FarmTypeController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>/
        /// <summary>
        /// Get all farmtypes
        /// </summary>
        /// <returns>IEnumerable<FarmTypesDto></returns>
        [SwaggerOperation("GetFarmTypes")]
        [ResponseType(typeof(IEnumerable<FarmTypeDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No farms types found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farm types are being returned.", typeof(IEnumerable<FarmTypeDto>))]
        public IHttpActionResult Get()
        {
            try
            {
                var value = db.FarmTypes.DefaultIfEmpty(null).ToList();
                if (value != null)
                {
                    var farmTypes = AutoMapper.Mapper.Map<List<FarmTypeDto>>(value);
                    Logger.Info("Returning {count} farmtypes", farmTypes.Count);
                    return Ok(farmTypes);
                }

                Logger.Info("No farmtypes found");
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>
        /// <summary>
        /// Update farm type
        /// </summary>
        /// <param name="farmTypeDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("UpdateFarmType")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The farm type data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Farm type updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No matching farm type found.")]
        public IHttpActionResult Put([FromBody]FarmTypeDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }

            try
            {
                var ft = db.FarmTypes.Find(value.ID);
                if (ft != null)
                {
                    db.FarmTypes.Add(ft);
                    db.Entry(ft).State = EntityState.Modified;
                    db.SaveChanges();
                    Logger.Info("Farm type updated: {@value1}", ft);
                    return StatusCode(HttpStatusCode.NoContent);
                }

                Logger.Info("Farm type data for {id} not found", value.ID);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

    }
}
