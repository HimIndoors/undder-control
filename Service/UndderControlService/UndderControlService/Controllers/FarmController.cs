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
        public IHttpActionResult GetFarmByID(int id)
        {
            var thisYear = DateTime.Now.Year;
            var result = Mapper.Map<FarmDto>(db.Farms.Where(f => f.ID == id));
            return Ok(result);
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a new farm
        /// </summary>
        /// <param name="farmDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("PostFarm")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The farm data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Farm uploaded successfully.")]
        public IHttpActionResult CreateFarm([FromBody]FarmDto farmDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Farm farm = Mapper.Map<Farm>(farmDto);

                db.Farms.Add(farm);
                db.SaveChanges();

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

                return InternalServerError(new InvalidOperationException(eve.Message + "\r\n" + String.Join("\r\n", errors.ToArray()), eve));
            }
            catch (Exception ex)
            {
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
        public IHttpActionResult UpdateFarm([FromBody]FarmDto farmDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 
            var farm = db.Farms.Find(farmDto.ID);
            if (farm != null)
            {
                db.Farms.Add(farm);
                db.Entry(farm).State = EntityState.Modified;
                db.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }

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

                return StatusCode(HttpStatusCode.NoContent);
            }

            return NotFound();
        }
    }
}