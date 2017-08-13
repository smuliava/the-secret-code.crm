﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheSecretCode.CRM.Classes;
using TheSecretCode.CRM.Infrastructure;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Controllers
{
    [RoutePrefix("Api/System-Users")]
    public class SystemUserController : ApiController
    {
        private AuthRepository _repository = new AuthRepository();
        private SystemUserManager _systemUserManager = null;
        private SystemUserManager SystemUserManager
        {
            get
            {
                return _systemUserManager ?? Request.GetOwinContext().GetUserManager<SystemUserManager>();
            }
        }

        public SystemUserController()
        {

        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(AuthenticationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repository.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpGet]
        [Route("{UserName}")]
        public async Task<IHttpActionResult> GetUserById(string userName)
        {
            var user = await SystemUserManager.FindByNameAsync(userName);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> GetUserById(Guid id)
        {
            var user = await SystemUserManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
