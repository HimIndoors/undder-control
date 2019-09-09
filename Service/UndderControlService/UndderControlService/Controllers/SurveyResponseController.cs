using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
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
    public class SurveyResponseController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>/5
        // Take the latest two responses
        /// <summary>
        /// Get all survey responses by Farm ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<FarmDto></returns>
        [SwaggerOperation("GetSurveyResponseByFarmID")]
        [ResponseType(typeof(IEnumerable<SurveyResponseDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "A User with the specified ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farms are being returned.", typeof(IEnumerable<SurveyResponseDto>))]
        public IHttpActionResult Get(int id)
        {
            var value = db.SurveyResponses.DefaultIfEmpty(null).Where(s => s.User_ID == id).OrderByDescending(s => s.SubmittedDate).Take(2).ToList();
            if (value != null && value.Count > 0)
            {
                var result = Mapper.Map<List<SurveyResponseDto>>(value);
                Logger.Info("Returning Survey Responses for Farm: {id}", id);
                return Ok(result);
            }

            Logger.Info("No Survey Responses found for Farm: {id}", id);
            return NotFound();
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a survey response
        /// </summary>
        /// <param name="SurveyResponseDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("PostSurveyResponse")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The survey response data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Survey response uploaded successfully.")]
        public IHttpActionResult Post([FromBody]SurveyResponseDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid: {@value1}", value);
                return BadRequest(ModelState);
            }
                
            try
            {
                SurveyResponse sr = Mapper.Map<SurveyResponse>(value);

                db.SurveyResponses.Add(sr);
                db.SaveChanges();
                Logger.Info("Survey {id} created successfully", sr.ID);
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
    }
}