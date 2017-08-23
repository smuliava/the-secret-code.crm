﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TheSecretCode.CRM.Infrastructure;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Controllers
{
    [RoutePrefix("Api/Menu/{MenuType}")]
    public class MenuController : ApiController
    {
        private async Task<Menu[]> GetMenuById(string menuType, Guid? parentId)
        {
            MenuDbContext db = new MenuDbContext();
            return await db.Menu
                .Where(menuItem => menuItem.ParentId == parentId && menuItem.MenuType == menuType)
                .OrderBy(menuItem => menuItem.Order)
                .ToArrayAsync();
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetMainMenu(string menuType)
        {
            return Ok(await GetMenuById(menuType, null));
        }
        [HttpGet]
        [Route("{ParentId:guid}")]
        public async Task<IHttpActionResult> GetChildMenuById(string menuType, Guid parentId)
        {
            return Ok(await GetMenuById(menuType, parentId));
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateMenuItems(string menuType, Menu[] newMenuItems)
        {
            if (newMenuItems.Length != 0 && ModelState.IsValid)
            {
                var db = new MenuDbContext();
                var parentIds = new List<Guid?>();
                for(int i = 0, lenght = newMenuItems.Length; i < lenght; i++)
                {
                    parentIds.Add(newMenuItems[i].ParentId);
                    newMenuItems[i].MenuType = menuType;
                }
                var menu = await db.Menu
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
                db.Menu.AddRange(newMenuItems);
                await db.SaveChangesAsync();
                return Ok(newMenuItems);
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> UpdateMenuItems(string menuType, CollectionRequest<Menu> newMenuItemsData)
        {
            if (ModelState.IsValid)
            {
                MenuDbContext db = new MenuDbContext();
                if (newMenuItemsData.Reorder)
                {
                    List<Guid?> parentIds = new List<Guid?>();
                    for (int i = 0, length = newMenuItemsData.Collection.Length; i < length; i++)
                    {
                        parentIds.Add(newMenuItemsData.Collection[i].ParentId);
                    }
                    var menu = await db.Menu
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
                    var menu = await db.Menu
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
        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> DeleteMenuItems(string menuType, Guid[] deleteMenuIds)
        {
            MenuDbContext db = new MenuDbContext();
            var oldMenu = await db.Menu
                .Where(menuItem => deleteMenuIds.Contains(menuItem.Id) && menuItem.MenuType == menuType)
                .ToArrayAsync();
            db.Menu.RemoveRange(oldMenu);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}