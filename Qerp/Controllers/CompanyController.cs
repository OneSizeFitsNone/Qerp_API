using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        public long _companyId;

        public CompanyController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

#warning CompanyController GetAllCompanies only on superuser
        //[HttpGet]
        //public async Task<ReturnResult> GetAll()
        //{
        //    return await CompanyMV.SelectAll();
        //}

        [HttpGet]
        public async Task<ReturnResult> Get()
        {
            return await CompanyMV.SelectById(_companyId);
        }

#warning CompanyController Post only on superuser
        //[HttpPost]
        //public async Task<ReturnResult> Post(CompanyMV company)
        //{
        //    return await company.Insert();
        //}

        [HttpPost]
        public async Task<ReturnResult> Post(CompanyMV company)
        {
            if (_companyId != company.Id) { new ReturnResult(false, "Access Denied", null); }
            return await company.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(CompanyMV company)
        {
            if (_companyId != company.Id) { new ReturnResult(false, "Access Denied", null); }
            return await company.Delete();
        }
    }
}
