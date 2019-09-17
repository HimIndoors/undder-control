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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>
        /// <summary>
        /// Get the latest active survey
        /// </summary>
        /// <returns>SurveyDto</returns>
        [SwaggerOperation("GetLatestSurvey")]
        [ResponseType(typeof(IEnumerable<SurveyDto>))]
        [SwaggerResponse(HttpStatusCode.OK, "The latest survey is being returned.", typeof(IEnumerable<SurveyDto>))]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var value = db.Surveys.DefaultIfEmpty(null).Where(s => s.Active).OrderByDescending(s => s.Version).FirstOrDefault();
                if (value != null)
                {
                    var survey = Mapper.Map<SurveyDto>(value);
                    Logger.Info("Returning survey: {@value1}", survey);
                    return Ok(survey);
                }

                Logger.Info("No survey found");
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        /// <summary>
        /// Get the specified survey.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SurveyDto</returns>
        /// [SwaggerOperation("GetSurveyByID")]
        [ResponseType(typeof(SurveyDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, "A Survey with the specified ID was not found.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified survey is being returned.", typeof(SurveyDto))]
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var value = db.Surveys.DefaultIfEmpty(null).Where(s => s.ID == id).SingleOrDefault();
                if (value != null)
                {
                    var survey = Mapper.Map<SurveyDto>(value);
                    Logger.Info("Returning survey: {@value1}", survey);
                    return Ok(survey);
                }

                Logger.Info("No survey found with ID: {id}", id);
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
        /// Import a new Survey.
        /// </summary>
        /// <param name="surveyDto"></param>
        /// <returns></returns>
        [SwaggerOperation("PostSurvey")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NoContent, "The survey was imported.")]
        public IHttpActionResult Post([FromBody]SurveyDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid: {@value1}", value);
                return BadRequest(ModelState);
            }  
                
            try
            {
                Survey survey = Mapper.Map<Survey>(value);

                db.Surveys.Add(survey);
                db.SaveChanges();
                Logger.Info("Survey {id} created successfully", survey.ID);

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