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
                var uniqueParentId = new Dictionary<string, bool>(10);
                for(int i = 0, lenght = newMenuItems.Length; i < lenght; i++)
                {
                    var parentId = newMenuItems[i].ParentId;
                    var parentIdStr = parentId.ToString();
                    if (!uniqueParentId.ContainsKey(parentIdStr))
                    {
                        uniqueParentId.Add(parentIdStr, true);
                        parentIds.Add(parentId);
                        newMenuItems[i].MenuType = menuType;
                    }

                }
                var menu = await db.Menu
                    .Where(menuItem => parentIds.Contains(menuItem.ParentId) && menuItem.MenuType == menuType)
                    .OrderByDescending(menuItem => menuItem.ParentId)
                    .OrderBy(menuItem => menuItem.ParentId.HasValue)
                    .ThenBy(menuItem => menuItem.Order)
                    .ToArrayAsync();
                Array.Sort<SystemMenu>(newMenuItems, (itemA, itemB) =>
                {
                    var parentIdComparision = Nullable.Compare<Guid>(itemA.ParentId, itemB.ParentId);
                    if (parentIdComparision == 0)
                    {
                        return Nullable.Compare<int>(itemA.Order, itemB.Order);
                    }
                    return parentIdComparision;
                });

                var leftIndex = newMenuItems[0].Order;
                var order = leftIndex;
                var rightIndex = 0;
                var leftLength = menu.Length;
                var rightLength = newMenuItems.Length;

                while (leftIndex < leftLength && rightIndex < rightLength)
                {
                    var previousLeftParentId = menu[leftIndex].ParentId;
                    var previousRightParentId = newMenuItems[rightIndex].ParentId;

                    if (previousLeftParentId == previousRightParentId && menu[leftIndex].Order < newMenuItems[rightIndex].Order)
                    {
                        leftIndex++;
                    }
                    else if (previousLeftParentId == previousRightParentId && menu[leftIndex].Order > newMenuItems[rightIndex].Order)
                    {
                        rightIndex++;
                    }
                    else if (previousLeftParentId == previousRightParentId && menu[leftIndex].Order == newMenuItems[rightIndex].Order)
                    {
                        order = newMenuItems[rightIndex++].Order + 1;
                    }
                    else if (menu[leftIndex].ParentId != newMenuItems[rightIndex].ParentId && menu[leftIndex].ParentId == previousLeftParentId)
                    {
                        rightIndex++;
                    }
                    else if (menu[leftIndex].ParentId != newMenuItems[rightIndex].ParentId && newMenuItems[rightIndex].ParentId == previousRightParentId)
                    {
                        leftIndex++;
                        order = 0;
                    }
                    menu[leftIndex].Order = order;
                }

                //db.SystemMenu.AddRange(newMenuItems);
                //await db.SaveChangesAsync();
                return Ok(menu.Concat(newMenuItems));
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