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
            if (newSystemMenuItems.Length != 0 && ModelState.IsValid)
            {
                var db = new SystemMenuContext();
                var parentIds = new List<Guid?>();
                for(int i = 0, lenght = newSystemMenuItems.Length; i < lenght; i++)
                {
                    parentIds.Add(newSystemMenuItems[i].ParentId);
                }
                var systemMenu = await db.SystemMenu
                    .Where(systemMenuItem => parentIds.Contains(systemMenuItem.ParentId))
                    .OrderByDescending(systemMenuItem => systemMenuItem.ParentId)
                    .OrderBy(systemMenuItem => systemMenuItem.ParentId.HasValue)
                    .ThenBy(systemMenuItem => systemMenuItem.Order)
                    .ToListAsync();
                var sortedNewSystemMenuItems = newSystemMenuItems
                    .OrderBy(systemMenuItem => systemMenuItem.ParentId)
                    .ThenBy(systemMenuItem => systemMenuItem.Order)
                    .ToList();
                var currentParentId = sortedNewSystemMenuItems[0].ParentId;
                bool sortedNewSystemMenuItemsEnd = false;
                for (int i = 0, j = 0, numberOfItemsPassedCurrentParentId = 0,
                    indexFirestItemCurrentParentIdInSystemMenu = 0, reorderWithoutQueue = 0,
                    count = systemMenu.Count; i < count; i++)
                {
                    if (sortedNewSystemMenuItemsEnd)
                    {
                        systemMenu[i].Order = i - indexFirestItemCurrentParentIdInSystemMenu + numberOfItemsPassedCurrentParentId;
                    }
                    else
                    {
                        if (reorderWithoutQueue <= 0 &&
                            (systemMenu[i].ParentId != currentParentId ||
                            sortedNewSystemMenuItems[j].ParentId == currentParentId &&
                            systemMenu[i].ParentId == currentParentId))
                        {
                            if (systemMenu[i].ParentId != currentParentId)
                            {
                                indexFirestItemCurrentParentIdInSystemMenu = i;
                                numberOfItemsPassedCurrentParentId = 0;
                                currentParentId = systemMenu[i].ParentId;
                                while (sortedNewSystemMenuItems[j].ParentId != currentParentId)
                                {
                                    j++;
                                }
                                reorderWithoutQueue = 0;
                            }
                            if (reorderWithoutQueue <= 0 &&
                                (i - indexFirestItemCurrentParentIdInSystemMenu) == sortedNewSystemMenuItems[j].Order)
                            {
                                do
                                {
                                    numberOfItemsPassedCurrentParentId++;
                                    j++;
                                    if (j == sortedNewSystemMenuItems.Count)
                                    {
                                        sortedNewSystemMenuItemsEnd = true;
                                        break;
                                    }
                                    if (sortedNewSystemMenuItems[j].ParentId != currentParentId)
                                    {
                                        reorderWithoutQueue = 0;
                                        break;
                                    }
                                    reorderWithoutQueue = sortedNewSystemMenuItems[j].Order - sortedNewSystemMenuItems[j - 1].Order - 1;
                                } while (reorderWithoutQueue == 0);
                            }
                        }
                        systemMenu[i].Order = i - indexFirestItemCurrentParentIdInSystemMenu + numberOfItemsPassedCurrentParentId;
                        reorderWithoutQueue--;
                    }
                }
                db.SystemMenu.AddRange(newSystemMenuItems);
                await db.SaveChangesAsync();
                return Ok(newSystemMenuItems);
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