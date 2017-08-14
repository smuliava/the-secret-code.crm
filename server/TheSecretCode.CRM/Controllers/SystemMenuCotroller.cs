using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Controllers
{
    internal class SystemMenuContext : DbContext
    {
        public SystemMenuContext() 
            : base("SystemMenuContext")
        {

        }

        public DbSet<SystemMenuModel> SystemMenu { get; set; }
    }
    [RoutePrefix("Api/System-Menu")]
    public class SystemMenuController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetSystemMenu()
        {
            SystemMenuContext db = new SystemMenuContext();
            var systemMenu = db.SystemMenu.Where(systemMenuItem => systemMenuItem.ParentId == null);
            return Ok(systemMenu);
        }
        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IHttpActionResult> GetSystemMenuById(Guid id)
        {
            SystemMenuContext db = new SystemMenuContext();
            var systemMenu = db.SystemMenu.Where(systemMenuItem => systemMenuItem.ParentId == id);
            return Ok(systemMenu);
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateSystemMenu(SystemMenuModel newMenuItem)
        {
            if (ModelState.IsValid)
            {
                SystemMenuContext db = new SystemMenuContext();
                newMenuItem.Id = Guid.NewGuid();
                newMenuItem.CreatedOn = DateTime.Now;
                newMenuItem.ModifiedOn = DateTime.Now;
                db.SystemMenu.Add(newMenuItem);
                db.SaveChanges();
                return Ok(newMenuItem);
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IHttpActionResult> UpdateSystemMenu(Guid id, SystemMenuModel newMenuItemData)
        {
            if(ModelState.IsValid)
            {
                SystemMenuContext db = new SystemMenuContext();
                var newMenuItem = await db.SystemMenu.FindAsync(id);
                newMenuItem.ParentId = newMenuItemData.ParentId;
                newMenuItem.ModifiedBySystemUserId = newMenuItemData.ModifiedBySystemUserId;
                newMenuItem.ModifiedOn = DateTime.Now;
                newMenuItem.Caption = newMenuItemData.Caption;
                newMenuItem.Title = newMenuItemData.Title;
                newMenuItem.Order = newMenuItemData.Order;
                db.SaveChanges();
                return Ok(newMenuItem);
            }
            return BadRequest();
        }
    }
}