using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;
using UndderControlService.Data.Entities;

namespace UndderControlService.Controllers
{
    public class FarmController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>
        /// <summary>
        /// Get all farms
        /// </summary>
        /// <returns>IEnumerable<FarmDto></returns>
        [SwaggerOperation("GetAllFarms")]
        [ResponseType(typeof(IEnumerable<FarmDto>))]
        [SwaggerResponse(HttpStatusCode.OK, "The farms are being returned.", typeof(IEnumerable<FarmDto>))]
        public IHttpActionResult Get()
        {
            var farms = Mapper.Map<List<FarmDto>>(db.Farms.ToList());
            Logger.Info("Returning {number} of farms", farms.Count);
            return Ok(farms);
        }

        // GET api/<controller>/5
        /// <summary>
        /// Get farm by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>FarmDto</returns>
        [SwaggerOperation("GetFarmByID")]
        [ResponseType(typeof(FarmDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No farm found for this ID.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farm is being returned.", typeof(FarmDto))]
        public IHttpActionResult Get(int id)
        {
            var value = db.Farms.DefaultIfEmpty(null).Where(f => f.ID == id).FirstOrDefault();

            if (value != null)
            {
                var result = Mapper.Map<FarmDto>(value);
                Logger.Info("Returning Farm {@value1}", value);
                return Ok(result);
            }

            Logger.Info("Farm data for {id} not found", id);
            return NotFound();
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a new farm
        /// </summary>
        /// <param name="farmDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("CreateFarm")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The farm data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Farm uploaded successfully.")]
        public IHttpActionResult Post([FromBody]FarmDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Farm modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }
                
            try
            {
                Farm farm = Mapper.Map<Farm>(value);
                db.Farms.Add(farm);
                db.SaveChanges();
                Logger.Info("Farm {id} created successfully", farm.ID);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbEntityValidationException eve)
            {
                List<String> errors = new List<string>();

                foreach (DbEntityValidationResult vr in eve.EntityValidationErrors)
                {
                    foreach (DbValidationError ve in vr.ValidationErrors)
                    {
                        string error = $"{vr.Entry.Entity.GetType().Name}.{ve.PropertyName}: {ve.ErrorMessage}";

                        if (!errors.Contains(error))
                            errors.Add(error);
                    }
                }

                Logger.Error(eve, eve.Message);
                return InternalServerError(new InvalidOperationException(eve.Message + "\r\n" + String.Join("\r\n", errors.ToArray()), eve));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>
        /// <summary>
        /// Update farm
        /// </summary>
        /// <param name="farmDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("UpdateFarm")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The farm data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Farm updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No matching farm found.")]
        public IHttpActionResult Put([FromBody]FarmDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Farm modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }
                
 
            var farm = db.Farms.Find(value.ID);
            if (farm != null)
            {
                db.Farms.Add(farm);
                db.Entry(farm).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("Farm Updated: {@value1}", farm);
                return StatusCode(HttpStatusCode.NoContent);
            }

            Logger.Info("Farm data for {id} not found", value.ID);
            return NotFound();
        }

        // DELETE api/<controller>/5
        /// <summary>
        /// Delete farm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("DeleteFarm")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Farm deleted.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No matching farm found.")]
        public IHttpActionResult Delete(int id)
        {
            var farm = db.Farms.Find(id);
            if (farm != null)
            {
                db.Farms.Remove(farm);
                db.SaveChanges();
                Logger.Info("Farm deleted: {id}", id);
                return StatusCode(HttpStatusCode.NoContent);
            }

            Logger.Info("Farm data for {id} not found", id);
            return NotFound();
        }
    }
}