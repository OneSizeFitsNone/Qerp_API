using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Qerp.Interfaces;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        public long _companyId;

        public ClientController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ClientMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ClientMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ClientMV client)
        {
            if (_companyId != client.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await client.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ClientMV client)
        {
            if (_companyId != client.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await client.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ClientMV client)
        {
            if (_companyId != client.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await client.Delete();
        }

        [HttpPost("search")]
        public async Task<ReturnResult> Search(ClientMV client)
        {
            return await client.Search(_companyId);
        }
    }
}
