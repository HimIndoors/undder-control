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
        // GET api/<controller>/5
        /// <summary>
        /// Get all survey responses by Farm ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<FarmDto></returns>
        [SwaggerOperation("GetSurveyResponseByFarmID")]
        [ResponseType(typeof(IEnumerable<SurveyResponseDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "A User with the specified ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified farms are being returned.", typeof(IEnumerable<SurveyResponseDto>))]
        public IHttpActionResult GetSurveyResponseByFarmID(int id)
        {
            var result = Mapper.Map<List<SurveyResponseDto>>(db.SurveyResponses.Where(s => s.User_ID == id).ToList());
            return Ok(result);
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
        public IHttpActionResult CreateFarm([FromBody]SurveyResponseDto srDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                SurveyResponse sr = Mapper.Map<SurveyResponse>(srDto);

                db.SurveyResponses.Add(sr);
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
    }
}