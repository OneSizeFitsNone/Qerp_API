using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : Controller
    {
        public long _companyId;

        public ProvinceController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProvinceMV.SelectAll(_companyId);
        }

        [HttpGet("GetByCountry")]
        public async Task<ReturnResult> GetByCountry(long countryId)
        {
            return await ProvinceMV.SelectByCountry(_companyId, countryId);
        }



    }


}
