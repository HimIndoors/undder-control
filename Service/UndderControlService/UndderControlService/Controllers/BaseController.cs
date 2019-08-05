using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UndderControlService.Data.Repositories;

namespace UndderControlService.Controllers
{
    public class BaseController : ApiController
    {
        protected EntitiesDbContext db = new EntitiesDbContext();
    }
}