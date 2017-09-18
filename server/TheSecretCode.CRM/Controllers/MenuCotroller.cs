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
                var menu = await db.SystemMenu
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