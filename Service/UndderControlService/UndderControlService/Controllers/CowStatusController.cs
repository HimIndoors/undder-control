using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;
using UndderControlService.Data.Entities;

namespace UndderControlService.Controllers
{
    public class CowStatusController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>/5
        /// <summary>
        /// Get cow status by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CowStatusDto</returns>
        [SwaggerOperation("GetCowStatusByID")]
        [ResponseType(typeof(CowStatusDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No cow status found for this farm.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified cow status are being returned.", typeof(CowStatusDto))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var value = db.CowStatus.DefaultIfEmpty(null).Where(x => x.ID == id).FirstOrDefault();
                if (value != null)
                {
                    var result = Mapper.Map<CowStatusDto>(value);
                    Logger.Info("Returning CowStatus {@value1}", value);
                    return Ok(result);
                }

                Logger.Info("CowStatus not found: {id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a new cow status
        /// </summary>
        /// <param name="CowStatusDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("CreateCowStatus")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The cow status data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Cow status uploaded successfully.")]
        public IHttpActionResult Post([FromBody]CowStatusDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("CowStatus modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }
                

            try
            {
                CowStatus cow = Mapper.Map<CowStatus>(value);

                db.CowStatus.Add(cow);
                db.SaveChanges();
                Logger.Info("CowStatus {id} created successfully", cow.ID);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbEntityValidationException eve)
            {
                List<string> errors = new List<string>();

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
        // Should only be called for a calving status but need to check for existing cow status
        // and if not found create a new one so we can keep the data. 
        // Future update needs to tie down these status updates to make sure they match somehow.
        /// <summary>
        /// Update cow status
        /// </summary>
        /// <param name="farmDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("UpdateCowStatus")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The cow status data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Cow status updated.")]
        public IHttpActionResult Put([FromBody]CowStatusDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid: {@value1}", value);
                return BadRequest(ModelState);
            }
                

            var cowStatus = db.CowStatus.Find(value.ID);
            if (cowStatus != null)
            {
                cowStatus.InfectedAtCalving = value.InfectedAtCalving;
                cowStatus.DateAddedCalving = value.DateAddedCalving;
                db.SaveChanges();
                Logger.Info("CowStatus Updated: {@value1}", cowStatus);

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                try
                {
                    CowStatus cow = Mapper.Map<CowStatus>(value);

                    db.CowStatus.Add(cow);
                    db.SaveChanges();

                    return StatusCode(HttpStatusCode.NoContent);
                }
                catch (DbEntityValidationException eve)
                {
                    List<string> errors = new List<string>();

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
                    Logger.Error(ex, ex.Message);
                    return InternalServerError(ex);
                }
            }
        }
    }
}
