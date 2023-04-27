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
    public class MilestoneController : Controller
    {

        public long _companyId;

        public MilestoneController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await MilestoneMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await MilestoneMV.Select(id, _companyId);
        }

        [HttpGet("SelectByApptype")]
        public async Task<ReturnResult> Get(long appTypeId, long id)
        {
            return await MilestoneMV.SelectByApptype(appTypeId, id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(MilestoneMV milestone)
        {
            if (milestone.Id != 0)
            {
                if (_companyId != milestone.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await milestone.Update();
            }
            else
            {
                milestone.CompanyId = _companyId;
                return await milestone.Insert();
            }
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(MilestoneMV milestone)
        {
            if (_companyId != milestone.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await milestone.Delete();
        }

        [HttpPost("search")]
        public async Task<ReturnResult> Search(MilestoneMV milestone)
        {
            return await milestone.Search(_companyId);
        }

    }
}
