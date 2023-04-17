using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;
using System.Threading.Tasks;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectController : Controller
    {
        public long _companyId;

        public ProspectController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProspectMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ProspectMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ProspectMV prospect)
        {
            if (prospect.Id != 0)
            {
                if (_companyId != prospect.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await prospect.Update();
            }
            else
            {
                prospect.CompanyId = _companyId;
                return await prospect.Insert();
            }
        }



        [HttpDelete]
        public async Task<ReturnResult> Delete(ProspectMV prospect)
        {
            if (_companyId != prospect.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await prospect.Delete();
        }
    }
}
