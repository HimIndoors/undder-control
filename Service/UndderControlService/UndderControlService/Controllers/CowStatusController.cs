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
    public class CowStatusController : BaseController
    {
        // GET api/<controller>/5
        /// <summary>
        /// Get all cows by Farm ID, filtered by calendar year.
        /// Only records with both dry-off and calving dates for the current year are returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<CowStatusDto></returns>
        [SwaggerOperation("GetCowsByID")]
        [ResponseType(typeof(IEnumerable<CowStatusDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No cow status found for this farm.")]
        [SwaggerResponse(HttpStatusCode.OK, "The specified cow status are being returned.", typeof(IEnumerable<CowStatusDto>))]
        public IHttpActionResult GetCowStatusByID(int id)
        { 
            var thisYear = DateTime.Now.Year;
            var result = AutoMapper.Mapper.Map<List<CowStatusDto>>(db.CowStatus.Where(
                c => c.Farm_ID == id
                && (c.DateAddedCalving.HasValue && c.DateAddedCalving.Value.Year == thisYear)
                && (c.DateAddedDryOff.HasValue && c.DateAddedDryOff.Value.Year == thisYear)
                ).ToList());
            return Ok(result);
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a new cow status
        /// </summary>
        /// <param name="CowStatusDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("CreateCow")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The cow status data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Cow status uploaded successfully.")]
        public IHttpActionResult CreateCow([FromBody]CowStatusDto csDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                CowStatus cow = Mapper.Map<CowStatus>(csDto);

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
                return InternalServerError(ex);
            }
        }
    }
}
