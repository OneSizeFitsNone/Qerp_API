﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientcontactController : Controller
    {
        public long _companyId;

        public ClientcontactController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        //[HttpGet]
        //public async Task<ReturnResult> GetAll()
        //{
        //    return await ClientcontactMV.SelectAll();
        //}

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ClientcontactMV.SelectById(id);
        }

        [HttpGet("GetByContact/{id}")]
        public async Task<ReturnResult> GetByContact(long id)
        {
            return await ClientcontactMV.SelectByContactId(id);
        }

        [HttpGet("GetByClient/{id}")]
        public async Task<ReturnResult> GetByClient(long id)
        {
            return await ClientcontactMV.SelectByClientId(id);
        }


        [HttpPost]
        public async Task<ReturnResult> Post(ClientcontactMV clientcontact)
        {
            if (clientcontact.Id != 0)
            {
                return await clientcontact.Update();
            }
            else
            {
                return await clientcontact.Insert();
            }
        }


        [HttpDelete]
        public async Task<ReturnResult> Delete(ClientcontactMV clientcontact)
        {
            return await clientcontact.Delete();
        }
    }
}
