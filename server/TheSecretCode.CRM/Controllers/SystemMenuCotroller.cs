using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Controllers
{
    [RoutePrefix("Api/SystemMenu")]
    public class SystemMenuController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetSystemMenu()
        {
            var systemMenuData = new StreamReader(@"d:\projects\crm\the-secret-code.crm\server\TheSecretCode.CRM\DataMocks\SystemMenu.json");
            string json = await systemMenuData.ReadToEndAsync();
            systemMenuData.Close();
            return Ok(JArray.Parse(json));
        }
        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IHttpActionResult> GetSystemMenuById(Guid id)
        {
            var systemMenuByIdData = new StreamReader(@"d:\projects\crm\the-secret-code.crm\server\TheSecretCode.CRM\DataMocks\SystemMenu.json");
            string json = await systemMenuByIdData.ReadToEndAsync();
            systemMenuByIdData.Close();
            return Ok(JArray.Parse(json));
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateSystemMenu(SystemMenuModel menuItem)
        {
            if (ModelState.IsValid)
            {
                menuItem.Id = Guid.NewGuid().ToString();
                return Ok(menuItem);
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IHttpActionResult> UpdateSystemMenu(Guid id, SystemMenuModel menuItemData)
        {
            if(ModelState.IsValid)
            {
                return Ok(menuItemData);
            }
            return BadRequest();
        }
        private IHttpActionResult json(object p)
        {
            throw new NotImplementedException();
        }
    }
}