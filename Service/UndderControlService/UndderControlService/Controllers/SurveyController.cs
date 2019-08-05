using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UndderControlLib.Dtos;
using UndderControlService.Data.Entities;

namespace UndderControlService.Controllers
{
    public class SurveyController : BaseController
    {
        // GET api/<controller>
        /// <summary>
        /// Get a list of surveys. Only one survey should be marked as Active, use this method to retrieve latest survey
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        [SwaggerOperation("GetAllSurveys")]
        [ResponseType(typeof(IEnumerable<SurveyDto>))]
        [SwaggerResponse(HttpStatusCode.OK, "The surveys are being returned.", typeof(IEnumerable<SurveyDto>))]
        [AllowAnonymous]
        public IHttpActionResult Get(bool activeOnly = true)
        {
            var survey = AutoMapper.Mapper.Map<List<SurveyDto>>(db.Surveys.Where(s => !activeOnly || s.Active));
            if (survey == null)
                return NotFound();

            return Ok(survey);
        }

        // GET api/<controller>/5
        /// <summary>
        /// Get the specified survey.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// [SwaggerOperation("GetSurveyByID")]
        [ResponseType(typeof(SurveyDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, "A Survey with the specified ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified survey is being returned.", typeof(SurveyDto))]
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            var survey = Mapper.Map<List<SurveyDto>>(db.Surveys.Where(s => s.ID == id).SingleOrDefault());
            if (survey == null)
                return NotFound();

            return Ok(survey);
        }

        // POST api/<controller>
        /// <summary>
        /// Import a new Survey.
        /// </summary>
        /// <param name="surveyDto"></param>
        /// <returns></returns>
        [SwaggerOperation("PostSurvey")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NoContent, "The survey was imported.")]
        public IHttpActionResult Post([FromBody]SurveyDto surveyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                Survey importSurvey = Mapper.Map<Survey>(surveyDto);

                db.Surveys.Add(importSurvey);
                db.SaveChanges();

                return Ok();
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