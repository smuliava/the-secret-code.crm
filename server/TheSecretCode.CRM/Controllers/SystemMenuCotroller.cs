using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TheSecretCode.CRM.Controllers
{
    [RoutePrefix("api/SystemMenu")]
    public class SystemMenuCotroller : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok();
        }

    }
}