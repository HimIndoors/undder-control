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
    public class UserController : BaseController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>IEnumerable<UserDto></returns>
        [SwaggerOperation("GetAllUsers")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        [SwaggerResponse(HttpStatusCode.OK, "The users are being returned.", typeof(IEnumerable<UserDto>))]
        public IHttpActionResult Get()
        {
            var users = Mapper.Map<List<UserDto>>(db.Users.ToList());
            Logger.Info("Returning {number} of users", users.Count);
            return Ok(users);
        }

        // GET api/<controller>/5
        /// <summary>
        /// Get user by Token.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserDto</returns>
        [SwaggerOperation("GetUserByToken")]
        [ResponseType(typeof(UserDto))]
        [SwaggerResponse(HttpStatusCode.OK, "The specified user is being returned.", typeof(UserDto))]
        [Route("api/user/{token}")]
        public IHttpActionResult Get(string token)
        {
            //Check for existing user and if not available create new and return.
            UserDto user = Mapper.Map<UserDto>(db.Users.DefaultIfEmpty(null).Where(u => u.Token == token).FirstOrDefault()); 
            if (user == null)
            {
                User u = new User
                {
                    Token = token
                };
                db.Users.Add(u);
                db.SaveChanges();
                user = Mapper.Map<UserDto>(u);
            }

            Logger.Info("User returned: {@value1}", user);
            return Ok(user);
        }

        // POST api/<controller>
        /// <summary>
        /// Sumbit a new user
        /// </summary>
        /// <param name="UserDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("CreateUser")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The user data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "User uploaded successfully.")]
        public IHttpActionResult Post([FromBody]UserDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }    

            try
            {
                User user = Mapper.Map<User>(value);

                db.Users.Add(user);
                db.SaveChanges();
                Logger.Info("User {id} created successfully", user.ID);
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
        /// Update user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("UpdateUser")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "The user data wasn't acceptable (improper formatting, etc.).")]
        [SwaggerResponse(HttpStatusCode.NoContent, "User updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No matching user found.")]
        public IHttpActionResult Put([FromBody]UserDto value)
        {
            if (!ModelState.IsValid)
            {
                Logger.Info("Modelstate invalid {@value1}", value);
                return BadRequest(ModelState);
            }

            var user = db.Users.Find(value.ID);
            if (user != null)
            {
                db.Users.Add(user);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("User updated: {@value1}", user);
                return StatusCode(HttpStatusCode.NoContent);
            }

            Logger.Info("User data for {id} not found", value.ID);
            return NotFound();
        }

        // DELETE api/<controller>/5
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        [SwaggerOperation("DeleteUser")]
        [SwaggerResponse(HttpStatusCode.NoContent, "User deleted.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No matching user found.")]
        public IHttpActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                Logger.Info("User deleted: {id}", id);
                return StatusCode(HttpStatusCode.NoContent);
            }

            Logger.Info("User data for {id} not found", id);
            return NotFound();
        }
    }
}
