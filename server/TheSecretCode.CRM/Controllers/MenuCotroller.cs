﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Controllers
{
    internal class MenuContext : DbContext
    {
        public MenuContext() 
            : base("SystemMenuContext")
        {

        }

        public DbSet<SystemMenu> SystemMenu { get; set; }
    }
    [RoutePrefix("Api/Menu")]
    public class MenuController : ApiController
    {
        private async Task<SystemMenu[]> GetMenuById(string menuType, Guid? parentId)
        {
            MenuContext db = new MenuContext();
            return await db.SystemMenu
                .Where(menuItem => menuItem.ParentId == parentId && menuItem.MenuType == menuType)
                .OrderBy(menuItem => menuItem.Order)
                .ToArrayAsync();
        }
        [HttpGet]
        [Route("{MenuType}")]
        public async Task<IHttpActionResult> GetMenu(string menuType)
        {
            return Ok(await GetMenuById(menuType, null));
        }
        [HttpGet]
        [Route("{MenuType}/{ParentId:guid}")]
        public async Task<IHttpActionResult> GetChildMenuById(string menuType, Guid parentId)
        {
            return Ok(await GetMenuById(menuType, parentId));
        }
        [HttpPost]
        [Route("{MenuType}")]
        public async Task<IHttpActionResult> CreateMenu(string menuType, SystemMenu[] newMenuItems)
        {
            if (newMenuItems.Length != 0 && ModelState.IsValid)
            {
                var db = new MenuContext();
                var parentIds = new List<Guid?>();
                for(int i = 0, lenght = newMenuItems.Length; i < lenght; i++)
                {
                    parentIds.Add(newMenuItems[i].ParentId);
                    newMenuItems[i].MenuType = menuType;
                }
                var menu = await db.SystemMenu
                    .Where(menuItem => parentIds.Contains(menuItem.ParentId) && menuItem.MenuType == menuType)
                    .OrderByDescending(menuItem => menuItem.ParentId)
                    .OrderBy(menuItem => menuItem.ParentId.HasValue)
                    .ThenBy(menuItem => menuItem.Order)
                    .ToArrayAsync();
                var sortedNewMenuItems = newMenuItems
                    .OrderBy(menuItem => menuItem.ParentId)
                    .ThenBy(menuItem => menuItem.Order)
                    .ToArray();
                var currentParentId = sortedNewMenuItems[0].ParentId;
                bool sortedNewMenuItemsEnd = false;
                for (int i = 0, j = 0, numberOfItemsPassedCurrentParentId = 0,
                    indexFirestItemCurrentParentIdInMenu = 0, reorderWithoutQueue = 0,
                    length = menu.Length; i < length; i++)
                {
                    if (sortedNewMenuItemsEnd)
                    {
                        menu[i].Order = i - indexFirestItemCurrentParentIdInMenu + numberOfItemsPassedCurrentParentId;
                    }
                    else
                    {
                        if (reorderWithoutQueue <= 0 &&
                            menu[i].ParentId != currentParentId ||
                            sortedNewMenuItems[j].ParentId == currentParentId &&
                            menu[i].ParentId == currentParentId)
                        {
                            if (menu[i].ParentId != currentParentId)
                            {
                                indexFirestItemCurrentParentIdInMenu = i;
                                numberOfItemsPassedCurrentParentId = 0;
                                currentParentId = menu[i].ParentId;
                                while (sortedNewMenuItems[j].ParentId != currentParentId)
                                {
                                    j++;
                                }
                            }
                            if ((i - indexFirestItemCurrentParentIdInMenu) == sortedNewMenuItems[j].Order)
                            {
                                do
                                {
                                    numberOfItemsPassedCurrentParentId++;
                                    j++;
                                    if (j == sortedNewMenuItems.Length)
                                    {
                                        sortedNewMenuItemsEnd = true;
                                        break;
                                    }
                                    if (sortedNewMenuItems[j].ParentId != currentParentId)
                                    {
                                        reorderWithoutQueue = 0;
                                        break;
                                    }
                                    reorderWithoutQueue = sortedNewMenuItems[j].Order - sortedNewMenuItems[j - 1].Order - 1;
                                } while (reorderWithoutQueue == 0);
                            }
                        }
                        menu[i].Order = i - indexFirestItemCurrentParentIdInMenu + numberOfItemsPassedCurrentParentId;
                        reorderWithoutQueue--;
                    }
                }
                db.SystemMenu.AddRange(newMenuItems);
                await db.SaveChangesAsync();
                return Ok(newMenuItems);
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("{MenuType}")]
        public async Task<IHttpActionResult> UpdateMenuItems(string menuType, CollectionRequest<SystemMenu> newMenuItemsData)
        {
            if (ModelState.IsValid)
            {
                MenuContext db = new MenuContext();
                if (newMenuItemsData.Reorder)
                {
                    List<Guid?> parentIds = new List<Guid?>();
                    for (int i = 0, length = newMenuItemsData.Collection.Length; i < length; i++)
                    {
                        parentIds.Add(newMenuItemsData.Collection[i].ParentId);
                    }
                    var menu = await db.SystemMenu
                    .Where(menuItem => parentIds.Contains(menuItem.ParentId) && menuItem.MenuType == menuType)
                    .OrderByDescending(menuItem => menuItem.ParentId)
                    .OrderBy(menuItem => menuItem.ParentId.HasValue)
                    .ThenBy(menuItem => menuItem.Order)
                    .ToArrayAsync();
                    var sortedCollection = newMenuItemsData.Collection
                        .OrderBy(menuItem => menuItem.ParentId)
                        .ThenBy(menuItem => menuItem.Order)
                        .ToArray();

                }
                else
                {
                    List<Guid> ids = new List<Guid>();
                    for (int i = 0, length = newMenuItemsData.Collection.Length; i < length; i++)
                    {
                        ids.Add(newMenuItemsData.Collection[i].Id);
                    }
                    var sortedCollection = newMenuItemsData.Collection
                        .OrderBy(menuItem => menuItem.Id)
                        .ToArray();
                    var menu = await db.SystemMenu
                        .Where(menuItem => ids.Contains(menuItem.Id) && menuItem.MenuType == menuType)
                        .OrderBy(menuItem => menuItem.Id)
                        .ToArrayAsync();
                    for (int i = 0, length = menu.Length; i < length; i++)
                    {
                        menu[i].ParentId = sortedCollection[i].ParentId;
                        menu[i].ModifiedBySystemUserId = sortedCollection[i].ModifiedBySystemUserId;
                        menu[i].Caption = sortedCollection[i].Caption;
                        menu[i].Title = sortedCollection[i].Title;
                    }
                }
                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}