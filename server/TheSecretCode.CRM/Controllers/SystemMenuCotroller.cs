﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public DbSet<SystemMenu> SystemMenu { get; set; }
    }
    [RoutePrefix("Api/System-Menu")]
    public class SystemMenuController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetSystemMenu()
        {
            SystemMenuContext db = new SystemMenuContext();
            var systemMenu = await db.SystemMenu
                .Where(systemMenuItem => systemMenuItem.ParentId == null)
                .OrderBy(systemMenuItem => systemMenuItem.Order)
                .ToArrayAsync();
            return Ok(systemMenu);
        }
        [HttpGet]
        [Route("{Id:guid}")]
        public async Task<IHttpActionResult> GetChildSystemMenuById(Guid id)
        {
            SystemMenuContext db = new SystemMenuContext();
            var systemMenu = await db.SystemMenu
                .Where(systemMenuItem => systemMenuItem.ParentId == id)
                .OrderBy(systemMenuItem => systemMenuItem.Order)
                .ToArrayAsync();
            return Ok(systemMenu);
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateSystemMenu(SystemMenu[] newSystemMenuItems)
        {
            if (ModelState.IsValid)
            {
                var db = new SystemMenuContext();
                var parentIds = new List<Guid?>();
                var uniqueParentIds = new Dictionary<string, bool>(10);
                for(int i = 0, lenght = newSystemMenuItems.Length; i < lenght; i++)
                {
                    var parentId = newSystemMenuItems[i].ParentId;
                    var parentIdKey = parentId.ToString();
                    if (!uniqueParentIds.ContainsKey(parentIdKey))
                    {
                        parentIds.Add(parentId);
                        uniqueParentIds.Add(parentIdKey, true);
                    }
                    
                }
                db.SystemMenu.AddRange(newSystemMenuItems);
                await db.SaveChangesAsync();
                var systemMenu = await db.SystemMenu
                    .Where(systemMenuItem => parentIds.Contains(systemMenuItem.ParentId))
                    .OrderBy(systemMenuItem => systemMenuItem.Order)
                    .ThenBy(systemMenuItem => systemMenuItem.CreatedOn)
                    .ToArrayAsync();
                return Ok(systemMenu);
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> UpdateSystemMenuItems(CollectionRequest<SystemMenu> newMenuItemsData)
        {
            if (ModelState.IsValid)
            {
                SystemMenuContext db = new SystemMenuContext();
                if (newMenuItemsData.Reorder)
                {

                }
                else
                {

                }
                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}